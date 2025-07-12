using ClaimsMS.Domain.Entities.Claims;
using ClaimsMS.Domain.Entities.Resolutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Core.Repositories.Resolutions
{
    public interface IResolutionRepository
    {
        Task AddAsync(ResolutionEntity claim);
        //Task DeleteAsync(ClaimId id);
       // Task<ClaimEntity?> UpdateAsync(ClaimEntity claim);
    }
}
