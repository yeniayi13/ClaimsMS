using ClaimsMS.Common.Dtos.Claim.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Claim.Queries
{
    public class GetClaimByStatusFilteredQuery : IRequest<List<GetClaimDto>>
    {
        public Guid? UserId { get; set; }
        public string? status { get; set; }
        public Guid? auctionId { get; set; }



        public GetClaimByStatusFilteredQuery(Guid? userId, string? status, Guid? auctionId)
        {
            UserId = userId;
            this.status = status;
            this.auctionId = auctionId;
        }

    }
}
