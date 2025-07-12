using ClaimsMS.Domain.Entities.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Core.Repositories.Claims
{
    public interface IClaimDeliveryRepository
    {
        Task AddAsync(ClaimDelivery claim);
        //Task DeleteAsync(ClaimId id);
        Task<ClaimDelivery?> UpdateAsync(ClaimDelivery claim);
    }
}
