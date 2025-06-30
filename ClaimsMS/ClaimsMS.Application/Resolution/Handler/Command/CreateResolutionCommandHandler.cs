using AutoMapper;
using ClaimsMS.Application.Resolution.Command;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Common.Dtos.Resolution.Response;
using ClaimsMS.Core.RabbitMQ;
using ClaimsMS.Core.Repositories.Claims;
using ClaimsMS.Core.Service.Auction;
using ClaimsMS.Core.Service.User;
using ClaimsMS.Domain.Entities.Claim.Enum;
using ClaimsMS.Domain.Entities.Claim.ValueObject;
using ClaimsMS.Domain.Entities.Claims.ValueObject;
using ClaimsMS.Domain.Entities.Claims;
using ClaimsMS.Infrastructure.Repositories.Claims;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration.Annotations;
using ClaimsMS.Domain.Entities.Resolution.ValueObject;
using ClaimsMS.Domain.Entities.Resolutions;
using ClaimsMS.Core.Repositories.Resolutions;
using ClaimsMS.Infrastructure.Exceptions;
using PaymentMS.Common.Dtos.Response;
using ClaimsMS.Core.Service.Notification;
using ClaimsMS.Application.Validator;
using FluentValidation;
using ClaimsMS.Core.Service.History;
using ClaimsMS.Common.Dtos.Product.Response;

namespace ClaimsMS.Application.Resolution.Handler.Command
{
    public class CreateResolutionCommandHandler: IRequestHandler<CreateResolutionCommand, Guid>
    {
        private readonly IClaimRepositoryMongo _claimRepositoryMongo;
        private readonly IClaimRepository _claimRepository;
        private readonly IEventBus<GetResolutionDto> _eventBus;
        private readonly IEventBus<GetClaimDto> _eventBusClaim;
        private readonly IResolutionRepository _resolutionRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
         private readonly IAuctionService _auctionService;
        private readonly INotificationService _notificationService;
        private readonly IHistoryService _historyService;
        public CreateResolutionCommandHandler(
            IClaimRepositoryMongo claimRepositoryMongo,
            IEventBus<GetResolutionDto> eventBus,
            IResolutionRepository resolutionRepository,
            IMapper mapper, INotificationService notificationService, IClaimRepository claimRepository
            , IEventBus<GetClaimDto> eventBusClaim, IUserService userService, IAuctionService auctionService
           ,IHistoryService historyService )
        {
            _claimRepositoryMongo = claimRepositoryMongo;
            _eventBus = eventBus;
            _mapper = mapper;
            _resolutionRepository = resolutionRepository;
            _notificationService = notificationService;
            _claimRepository = claimRepository ;
            _eventBusClaim = eventBusClaim;
            _userService = userService;
            _auctionService = auctionService;
            _historyService = historyService;

        }
        public async Task<Guid> Handle(CreateResolutionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await ValidateInputAsync(request, cancellationToken);

                var claim = await GetClaimAsync(request.ClaimId);
                var user = await _userService.BidderExists(claim.ClaimUserId.Value);

                var resolution = BuildResolutionEntity(request);
                var resolutionDto = _mapper.Map<GetResolutionDto>(resolution);

                await SaveResolutionAsync(resolution, resolutionDto);
                await NotifyUserAsync(user.UserId, resolutionDto.ResolutionDescription);

                await UpdateClaimStatusAsync(claim);
                // Registrar la actividad en el historial
                var history = new GetHistoryDto
                {
                    Id = Guid.NewGuid(),
                    UserId = user.UserId,
                    Action = $"Solucionaste el reclamo {request.Resolution.ClaimId} ",
                    Timestamp = DateTime.UtcNow
                };
                await _historyService.AddActivityHistoryAsync(history);
                return resolution.ResolutionId.Value;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error a crea la resolucion del reclamo: {ex.Message}", ex);
            }
        }

        private async Task ValidateInputAsync(CreateResolutionCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateResolutionValidator();
            var result = await validator.ValidateAsync(request.Resolution, cancellationToken);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        private async Task<ClaimEntity> GetClaimAsync(Guid claimId)
        {
            var claim = await _claimRepositoryMongo.GetByIdAsync(claimId);
            if (claim == null)
                throw new ClaimNotFoundException($"No claim found with ID {claimId}");

            return claim;
        }

        private ResolutionEntity BuildResolutionEntity(CreateResolutionCommand request)
        {
            return new ResolutionEntity(
                ResolutionId.Create(Guid.NewGuid()),
                ClaimId.Create(request.ClaimId),
                ResolutionDescription.Create(request.Resolution.ResolutionDescription)
            );
        }

        private async Task SaveResolutionAsync(ResolutionEntity resolution, GetResolutionDto resolutionDto)
        {
            await _resolutionRepository.AddAsync(resolution);
            await _eventBus.PublishMessageAsync(resolutionDto, "resolutionQueue", "RESOLUTION_CREATED");
        }

        private async Task NotifyUserAsync(Guid userId, string description)
        {
            var notification = new GetNotificationDto
            {
                NotificationId = Guid.NewGuid(),
                NotificationUserId = userId,
                NotificationSubject = "Your Claim Has Been Resolved",
                NotificationMessage = $"Your claim has been reviewed. A resolution has been issued: \"{description}\". Please check your account for more details.",
                NotificationDateTime = DateTime.UtcNow,
                NotificationStatus = "Sent"
            };

            await _notificationService.SendNotificationAsync(notification);
        }

        private async Task UpdateClaimStatusAsync(ClaimEntity claim)
        {
            claim.StatusClaim = StatusClaim.Resuelto;

            await _claimRepository.UpdateAsync(claim);

            var claimDto = _mapper.Map<GetClaimDto>(claim);
            await _eventBusClaim.PublishMessageAsync(claimDto, "claimQueue", "CLAIM_UPDATED");
        }


    }
}
