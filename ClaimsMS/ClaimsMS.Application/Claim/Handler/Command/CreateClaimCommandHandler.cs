using AutoMapper;
using ClaimsMS.Application.Claim.Command;
using ClaimsMS.Application.Validator;
using ClaimsMS.Common.Dtos.Claim.Request;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Common.Dtos.Product.Response;
using ClaimsMS.Core.RabbitMQ;
using ClaimsMS.Core.Repositories.Claims;
using ClaimsMS.Core.Service.Auction;
using ClaimsMS.Core.Service.History;
using ClaimsMS.Core.Service.Notification;
using ClaimsMS.Core.Service.User;
using ClaimsMS.Domain.Entities.Claim.Enum;
using ClaimsMS.Domain.Entities.Claim.ValueObject;
using ClaimsMS.Domain.Entities.Claims;
using ClaimsMS.Domain.Entities.Claims.ValueObject;
using ClaimsMS.Infrastructure.Exception;
using ClaimsMS.Infrastructure.Exceptions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Claim.Handler.Command
{
    public class CreateClaimCommandHandler : IRequestHandler<CreateClaimCommand, Guid>
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IEventBus<GetClaimDto> _eventBus;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuctionService _auctionService;
        private readonly IHistoryService _historyService;



        public CreateClaimCommandHandler(
            IClaimRepository claimRepository,
            IEventBus<GetClaimDto> eventBus,
            IUserService userService,
            IMapper mapper,
            IAuctionService auctionService, IHistoryService historyService)
        {
            _claimRepository = claimRepository;
            _eventBus = eventBus;
            _userService = userService;
            _mapper = mapper;
            _auctionService = auctionService;
            _historyService = historyService;

        }
        public async Task<Guid> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Validate(request.Claim, cancellationToken);
                await ValidateAuction(request.AuctionId);
                await ValidateUser(request.UserId);

                var claim = CrearEntity(request);
                await SaveClaim(claim);
                await PublishEventClaimCreated(claim);
                await RecordActivityInHistory(request.UserId);

                return claim.ClaimId.Value;
            }
            catch (ValidationException)
            {
                throw; // Se lanza sin envolver para conservar los errores específicos de validación
            }
            catch (UserNotFoundException)
            {
                throw; // El usuario no existe, se lanza como excepción controlada
            }
            catch (Exception)
            {
                throw; // Excepciones inesperadas
            }
        }

        //Metodos privados


        //Valida el objeto CreateClaimDto usando FluentValidation
        private async void Validate(CreateClaimDto claimDto, CancellationToken cancellationToken)
        {
            var validator = new CreateClaimValidator();
            var validationResult = await validator.ValidateAsync(claimDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }

        // Valida si la subasta existe
        private async Task ValidateAuction(Guid auctionId)
        {
            var auctionExists = await _auctionService.AuctionExists(auctionId);
            if (!auctionExists)
                throw new AuctionNotFoundException($"No existe Subasta con el ID: {auctionId}");
        }

        // Valida si el usuario existe
        private async Task ValidateUser(Guid userId)
        {
            var bidder = await _userService.BidderExists(userId);
            if (bidder == null)
                throw new UserNotFoundException($"No existe usuario con el ID: {userId}");
        }

        // Crea una entidad ClaimEntity 
        private ClaimEntity CrearEntity(CreateClaimCommand request)
        {
            return new ClaimEntity(
                ClaimId.Create(Guid.NewGuid()),
                ClaimAuctionId.Create(request.AuctionId),
                ClaimDescription.Create(request.Claim.ClaimDescription),
                ClaimReason.Create(request.Claim.ClaimReason),
                StatusClaim.Pendiente, // Estado inicial por defecto
                ClaimEvidence.FromBase64(request.Claim.ClaimEvidence),
                ClaimUserId.Create(request.UserId)
            );
        }

        // Guarda la entidad ClaimEntity en el repositorio
        private async Task SaveClaim(ClaimEntity claim)
        {
            await _claimRepository.AddAsync(claim);
        }

        // Publica un evento ClaimCreated en el bus de eventos
        private async Task PublishEventClaimCreated(ClaimEntity claim)
        {
            var claimDto = _mapper.Map<GetClaimDto>(claim);
            await _eventBus.PublishMessageAsync(claimDto, "claimQueue", "CLAIM_CREATED");
        }

        // Registra la actividad del usuario en el historial
        private async Task RecordActivityInHistory(Guid userId)
        {
            var history = new GetHistoryDto
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Action = "Iniciaste un nuevo reclamo",
                Timestamp = DateTime.UtcNow
            };

            await _historyService.AddActivityHistoryAsync(history);
        }


    }
}