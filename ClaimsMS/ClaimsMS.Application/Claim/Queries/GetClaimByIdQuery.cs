using ClaimsMS.Common.Dtos.Claim.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Claim.Queries
{
    public class GetClaimByIdQuery : IRequest<GetClaimDto>
    {
        public Guid? ClaimId { get; set; }
      

    }
}
