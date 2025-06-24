using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Claim.Command
{
    public class UpdateStatusClaimCommand: IRequest<Guid>
    {
        public Guid ClaimId { get; set; }

        public UpdateStatusClaimCommand(Guid claimId)
        {
            ClaimId = claimId;
        }
    }
}
