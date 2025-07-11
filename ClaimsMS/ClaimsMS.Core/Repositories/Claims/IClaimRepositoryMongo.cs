using ClaimsMS.Domain.Entities.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Core.Repositories.Claims
{
    public interface IClaimRepositoryMongo
    {
        Task<ClaimEntity?> GetByIdAsync(Guid id);
        Task<List<ClaimEntity?>> GetByStatusClaimsAsync(Guid? userId = null, Guid? auctionId = null, string? status = null);

        Task<List<ClaimEntity>> GetAllClaim();
    }
}
