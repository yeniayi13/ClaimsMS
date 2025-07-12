using ClaimsMS.Common.Dtos.Claim.Request;
using ClaimsMS.Common.Dtos.Resolution.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Resolution.Command
{
    public class CreateResolutionCommand : IRequest<Guid>
    {
        public CreateResolutionDto Resolution { get; set; }
        public Guid ClaimId { get; set; }

        public string TypeClaim { get; set; }

        public CreateResolutionCommand(CreateResolutionDto resolution, Guid claimgId, string typeClaim)
        {
            Resolution = resolution;
            ClaimId = claimgId;
            TypeClaim = typeClaim;
        }
    }
}
