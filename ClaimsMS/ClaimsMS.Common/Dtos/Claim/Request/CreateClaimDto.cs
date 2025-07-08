using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClaimsMS.Common.Dtos.Claim.Request
{
    public class CreateClaimDto
    {
        [JsonIgnore]
        public Guid? ClaimId { get; set; }
        public Guid ClaimAuctionId { get; set; }
        public string ClaimDescription { get; set; }
        public string ClaimReason { get; set; }

        [JsonIgnore]
        public string? StatusClaim { get; set; }
        public string ClaimEvidence { get; set; }

        public Guid ClaimUserId { get; set; }
    }
}
