using AutoMapper;
using ClaimsMS.Application.Claim.Command;
using ClaimsMS.Application.Validator;
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
            IAuctionService auctionService,IHistoryService historyService )
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
                //Valido los datos de entrada
                var validator = new CreateClaimValidator();
                var validationResult = await validator.ValidateAsync(request.Claim, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors); // No lo capturamos en un Exception genérico
                }

                // var auctionExists = await _auctionService.AuctionExists(request.Claim.AuctionId.value, request.UserId);
                // if (!auctionExists) throw new AuctionNotFoundException($"Auction with id {request.Claim.AuctionId.value} not found");

              

                // var autioneer = await _userService.AuctioneerExists(request.UserId);
                // var bidder = await _userService.BidderExists(request.UserId);


                // if (user == null) throw new UserNotFoundException($"user with id {request.UserId} not found");

                // Crear la entidad Claim
                var claim = new ClaimEntity(
                    ClaimId.Create(Guid.NewGuid()),
                    ClaimAuctionId.Create(request.AuctionId),
                    ClaimDescription.Create(request.Claim.ClaimDescription),
                    ClaimReason.Create(request.Claim.ClaimReason),
                    StatusClaim.Pendiente,
                    ClaimEvidence.FromBase64(request.Claim.ClaimEvidence),
                    ClaimUserId.Create(request.UserId)
                );

                var claimDto = _mapper.Map<GetClaimDto>(claim);

                // Guardar el producto en el repositorio
                await _claimRepository.AddAsync(claim);
                await _eventBus.PublishMessageAsync(claimDto, "claimQueue", "CLAIM_CREATED");
                // Registrar la actividad en el historial
                var history = new GetHistoryDto
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Action = $"Iniciaste un nuevo reclamo",
                    Timestamp = DateTime.UtcNow
                };
                await _historyService.AddActivityHistoryAsync(history);

                // Retornar el ID del producto registrado
                return claim.ClaimId.Value;
            }
            catch (ValidationException ex)
            {
                throw;

            }
            catch (UserNotFoundException ex)
            {
                throw;

            }
            catch (Exception ex)
            {
                throw;

            }
        }
    }
}
