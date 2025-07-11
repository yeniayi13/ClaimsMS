using AutoMapper;
using ClaimsMS.Application.Claim.Command;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Core.RabbitMQ;
using ClaimsMS.Core.Repositories.Claims;
using ClaimsMS.Domain.Entities.Claim.Enum;
using ClaimsMS.Infrastructure.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Claim.Handler.Command
{
    public class UpdateStatusClaimDeliveryCommandHandler : IRequestHandler<UpdateStatusClaimDeliveryCommand, Guid>
    {
        private readonly IClaimDeliveryRepositoryMongo _claimRepositoryMongo;
        private readonly IClaimDeliveryRepository _claimRepository;
        private readonly IEventBus<GetClaimDeliveryDto> _eventBus;
        private readonly IMapper _mapper;


        public UpdateStatusClaimDeliveryCommandHandler(
            IClaimDeliveryRepositoryMongo claimRepositoryMongo,
            IEventBus<GetClaimDeliveryDto> eventBus,
            IClaimDeliveryRepository claimRepository, IMapper mapper)
        {
            _claimRepositoryMongo = claimRepositoryMongo;
            _eventBus = eventBus;
            _claimRepository = claimRepository;
            _mapper = mapper;

        }

        public async Task<Guid> Handle(UpdateStatusClaimDeliveryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var claim = await _claimRepositoryMongo.GetByIdAsync(request.ClaimId);
                if (claim == null)
                {
                    throw new ClaimNotFoundException($"El reclamo con {request.ClaimId} no existe.");
                }
                claim.StatusClaim = StatusClaim.Revision;
                await _claimRepository.UpdateAsync(claim);
                await _eventBus.PublishMessageAsync(_mapper.Map<GetClaimDeliveryDto>(claim), "claimDeliveryQueue", "CLAIMDELIVERY_UPDATED");


                return claim.ClaimId.Value;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new InvalidOperationException("Error al actualizar el estado del reclamo.", ex);
            }
        }


    }
}
