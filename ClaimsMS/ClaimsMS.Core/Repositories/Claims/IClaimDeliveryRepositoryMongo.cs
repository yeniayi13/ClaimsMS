using ClaimsMS.Domain.Entities.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Core.Repositories.Claims
{
    public interface IClaimDeliveryRepositoryMongo
    {
        Task<ClaimDelivery?> GetByIdAsync(Guid id);
        Task<List<ClaimDelivery?>> GetByStatusClaimsAsync(Guid? userId = null, Guid? deliveryId = null, string? status = null);

        Task<List<ClaimDelivery>> GetAllClaimDelivery();
    }
}
