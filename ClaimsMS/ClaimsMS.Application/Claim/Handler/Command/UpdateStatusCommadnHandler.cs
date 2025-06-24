using AutoMapper;
using ClaimsMS.Application.Claim.Command;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Common.Dtos.Resolution.Response;
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
    public class UpdateStatusCommadnHandler : IRequestHandler<UpdateStatusClaimCommand, Guid>
    {
        private readonly IClaimRepositoryMongo _claimRepositoryMongo;
        private readonly IClaimRepository _claimRepository;
        private readonly IEventBus<GetClaimDto> _eventBus;
        private readonly IMapper _mapper;

        public UpdateStatusCommadnHandler(
            IClaimRepositoryMongo claimRepositoryMongo,
            IEventBus<GetClaimDto> eventBus,
            IClaimRepository claimRepository, IMapper mapper)
        {
            _claimRepositoryMongo = claimRepositoryMongo;
            _eventBus = eventBus;
            _claimRepository = claimRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(UpdateStatusClaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var claim = await _claimRepositoryMongo.GetByIdAsync(request.ClaimId);
                if (claim == null)
                {
                    throw new ClaimNotFoundException($"El reclamo con {request.ClaimId} no existe.");
                }
                claim.StatusClaim = StatusClaim.Pendiente;
                await _claimRepository.UpdateAsync(claim);
                await _eventBus.PublishMessageAsync(_mapper.Map<GetClaimDto>(claim), "claimQueue", "CLAIM_UPDATED");
                return claim.ClaimId.Value;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new InvalidOperationException("Error updating claim status.", ex);
            }
        }


    }
}
