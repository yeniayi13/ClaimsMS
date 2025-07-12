using ClaimsMS.Domain.Entities.Claims;
using ClaimsMS.Domain.Entities.Resolutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Core.Repositories.Resolutions
{
    public interface IResolutionRepositoryMongo
    {
        Task<List<ResolutionEntity?>> GetAllByFiltredResolutionAsync(Guid? claimId, Guid? id = null);
    }
}
