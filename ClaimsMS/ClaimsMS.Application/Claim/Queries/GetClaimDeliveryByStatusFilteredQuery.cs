using ClaimsMS.Common.Dtos.Claim.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Application.Claim.Queries
{
    public class GetClaimDeliveryByStatusFilteredQuery : IRequest<List<GetClaimDeliveryDto>>
    {
        public Guid? UserId { get; set; }
        public string? status { get; set; }
        public Guid? deliveryId { get; set; }

        public GetClaimDeliveryByStatusFilteredQuery(Guid? userId, string? status, Guid? deliveryId)
        {
            UserId = userId;
            this.status = status;
            this.deliveryId = deliveryId;
        }

    }
}
