using ClaimsMS.Common.Primitives;
using ClaimsMS.Domain.Entities.Claim.ValueObject;
using ClaimsMS.Domain.Entities.Claims;
using ClaimsMS.Domain.Entities.Resolution.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Domain.Entities.Resolutions
{
    public  class ResolutionEntity: AggregateRoot
    {
        public ResolutionId ResolutionId { get; private set; }
        public ClaimId? ClaimId { get; private set; }

        public ClaimId? ClaimDeliveryId { get; private set; }
        public ResolutionDescription ResolutionDescription { get; private set; }
        public ClaimEntity? Claim { get; private set; }    
        public ClaimDelivery? ClaimDelivery { get;  set; }
        public ResolutionEntity(ResolutionId resolutionId, ClaimId claimId, ResolutionDescription resolutionDescription )
        {
            ResolutionId = resolutionId;
            ClaimId = claimId;
            ResolutionDescription = resolutionDescription;
        }

        public ResolutionEntity(ResolutionId resolutionId,  ResolutionDescription resolutionDescription, ClaimId claimId)
        {
            ResolutionId = resolutionId;
            ClaimDeliveryId = claimId;
            ResolutionDescription = resolutionDescription;
        }

        private ResolutionEntity() { }
    }
}
