using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Claim.Command
{
    public class UpdateStatusClaimDeliveryCommand : IRequest<Guid>
    {
        public Guid ClaimId { get; set; }

        public UpdateStatusClaimDeliveryCommand(Guid claimId)
        {
            ClaimId = claimId;
        }
    }
}
