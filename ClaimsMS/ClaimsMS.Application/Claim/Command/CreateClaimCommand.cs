using ClaimsMS.Common.Dtos.Claim.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Claim.Command
{
    public class CreateClaimCommand : IRequest<Guid>
    {
        public CreateClaimDto Claim { get; set; }
        public Guid UserId { get; set; }

        public Guid AuctionId { get; set; }

        public CreateClaimCommand(CreateClaimDto claim, Guid userId, Guid auctionId)
        {
            Claim = claim;
            UserId = userId;
            AuctionId = auctionId;
        }
    }
}
