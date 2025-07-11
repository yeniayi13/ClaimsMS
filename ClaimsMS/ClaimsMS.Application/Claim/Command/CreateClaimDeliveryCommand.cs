using ClaimsMS.Common.Dtos.Claim.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Claim.Command
{
    public class CreateClaimDeliveryCommand : IRequest<Guid>
    {
        public CreateClaimDeliveryDto Claim { get; set; }
        public Guid UserId { get; set; }

        public Guid DeliveryId { get; set; }

        public CreateClaimDeliveryCommand(CreateClaimDeliveryDto claim, Guid userId, Guid deliveryId)
        {
            Claim = claim;
            UserId = userId;
            DeliveryId = deliveryId;
        }
    }
}
