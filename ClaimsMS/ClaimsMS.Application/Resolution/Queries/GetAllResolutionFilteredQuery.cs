using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Common.Dtos.Resolution.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Resolution.Queries
{
    public class GetAllResolutionFilteredQuery : IRequest<List<GetResolutionDto>>
    {
        public Guid? ClaimId { get; set; }
        public Guid? AuctionId { get; set; }

        public Guid? ResolutionId { get; set; }

    }
}
