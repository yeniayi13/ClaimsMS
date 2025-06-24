using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Common.Dtos.Claim.Response
{
    public class GetClaimDto
    {
        public Guid ClaimId { get;  set; }
        public Guid ClaimAuctionId { get;  set; }
        public string ClaimDescription { get;  set; }
        public string ClaimReason { get;  set; }
        public string StatusClaim { get; set; }
        public string ClaimEvidence { get;  set; }

        public Guid ClaimUserId { get; set; }
    }
}
