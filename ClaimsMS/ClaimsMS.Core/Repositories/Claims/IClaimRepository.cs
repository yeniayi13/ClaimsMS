using ClaimsMS.Domain.Entities.Claim.ValueObject;
using ClaimsMS.Domain.Entities.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Core.Repositories.Claims
{
    public interface IClaimRepository
    {
        Task AddAsync(ClaimEntity claim);
        //Task DeleteAsync(ClaimId id);
        Task<ClaimEntity?> UpdateAsync(ClaimEntity claim);
    }
}
