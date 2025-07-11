using ClaimsMS.Common.Primitives;
using ClaimsMS.Domain.Entities.Claim.Enum;
using ClaimsMS.Domain.Entities.Claim.ValueObject;
using ClaimsMS.Domain.Entities.Claims.ValueObject;
using ClaimsMS.Domain.Entities.Resolutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Domain.Entities.Claims
{
    public class ClaimDelivery : AggregateRoot
    {
        public ClaimId ClaimId { get; private set; }
        public ClaimDeliveryId ClaimDeliveryId { get; private set; }
        public ClaimDescription ClaimDescription { get; private set; }
        public ClaimReason ClaimReason { get; private set; }
        public StatusClaim StatusClaim { get; set; }
        public ClaimEvidence ClaimEvidence { get; private set; }
        public ResolutionEntity? Resolution { get; private set; }
        public ClaimUserId ClaimUserId { get; private set; }
        public ClaimDelivery(ClaimId claimId,
            ClaimDeliveryId claimDeliveryId,
            ClaimDescription claimDescription,
            ClaimReason claimReason,
            StatusClaim statusClaim, ClaimEvidence claimEvidence, ClaimUserId claimUserId)
        {
            ClaimId = claimId;
            ClaimDeliveryId = claimDeliveryId;
            ClaimDescription = claimDescription;
            ClaimReason = claimReason;
            StatusClaim = statusClaim;
            ClaimEvidence = claimEvidence;
            ClaimUserId = claimUserId;
        }

        public ClaimDelivery(ClaimId claimId,
             ClaimDeliveryId claimDeliveryId,
             ClaimDescription claimDescription,
             ClaimReason claimReason,
             StatusClaim statusClaim, ClaimUserId claimUserId)
        {
            ClaimId = claimId;
            ClaimDeliveryId = claimDeliveryId;
            ClaimDescription = claimDescription;
            ClaimReason = claimReason;
            StatusClaim = statusClaim;
            ClaimEvidence = null; // Initialize with null or a default value if needed
            ClaimUserId = claimUserId;
        }
    }
}
