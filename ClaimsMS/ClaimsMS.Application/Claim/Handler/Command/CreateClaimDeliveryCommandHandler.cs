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
using ClaimsMS.Core.Service.User;
using ClaimsMS.Domain.Entities.Claim.Enum;
using ClaimsMS.Domain.Entities.Claim.ValueObject;
using ClaimsMS.Domain.Entities.Claims.ValueObject;
using ClaimsMS.Domain.Entities.Claims;
using ClaimsMS.Infrastructure.Exception;
using ClaimsMS.Infrastructure.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClaimsMS.Application.Claim.Handler.Command
{
    public class CreateClaimDeliveryCommandHandler : IRequestHandler<CreateClaimDeliveryCommand, Guid>
    {
        private readonly IClaimDeliveryRepository _claimRepository;
        private readonly IEventBus<GetClaimDeliveryDto> _eventBus;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuctionService _auctionService;
        private readonly IHistoryService _historyService;



        public CreateClaimDeliveryCommandHandler(
            IClaimDeliveryRepository claimRepository,
            IEventBus<GetClaimDeliveryDto> eventBus,
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
        public async Task<Guid> Handle(CreateClaimDeliveryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Validate(request.Claim, cancellationToken);
               // await ValidateDelivery(request.DeliveryId);
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
        private async void Validate(CreateClaimDeliveryDto claimDto, CancellationToken cancellationToken)
        {
            var validator = new CreateClaimDeliveryValidator();
            var validationResult = await validator.ValidateAsync(claimDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }

        // Valida si la subasta existe
        private async Task ValidateDelivery(Guid deliveryId)
        {
            var auctionExists = await _auctionService.DeliveryById(deliveryId);
            if (!auctionExists)
                throw new AuctionNotFoundException($"No existe Entrega con el ID: {deliveryId}");
        }

        // Valida si el usuario existe
        private async Task ValidateUser(Guid userId)
        {
            var bidder = await _userService.BidderExists(userId);
            if (bidder == null)
                throw new UserNotFoundException($"No existe usuario con el ID: {userId}");
        }

        // Crea una entidad ClaimEntity 
        private ClaimDelivery CrearEntity(CreateClaimDeliveryCommand request)
        {
            return new ClaimDelivery(
                ClaimId.Create(Guid.NewGuid()),
                ClaimDeliveryId.Create(request.DeliveryId),
                ClaimDescription.Create(request.Claim.ClaimDescription),
                ClaimReason.Create(request.Claim.ClaimReason),
                StatusClaim.Pendiente, // Estado inicial por defecto
                ClaimEvidence.FromBase64(request.Claim.ClaimEvidence),
                ClaimUserId.Create(request.UserId)
            );
        }

        // Guarda la entidad ClaimEntity en el repositorio
        private async Task SaveClaim(ClaimDelivery claim)
        {
            await _claimRepository.AddAsync(claim);
        }

        // Publica un evento ClaimCreated en el bus de eventos
        private async Task PublishEventClaimCreated(ClaimDelivery claim)
        {
            var claimDto = _mapper.Map<GetClaimDeliveryDto>(claim);
            await _eventBus.PublishMessageAsync(claimDto, "claimDeliveryQueue", "CLAIMDELIVERY_CREATED");
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