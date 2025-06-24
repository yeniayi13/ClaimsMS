using AutoMapper;
using ClaimsMS.Core.Database;
using ClaimsMS.Core.Repositories.Claims;
using ClaimsMS.Domain.Entities.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Infrastructure.Repositories.Claims
{
    public class ClaimRepository: IClaimRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper; // Agregar el Mapper

        public ClaimRepository(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }
        public async Task AddAsync(ClaimEntity claim)
        {
            await _dbContext.Claims.AddAsync(claim);
            await _dbContext.SaveEfContextChanges("");
        }
        public async Task<ClaimEntity?> UpdateAsync(ClaimEntity claim)
        {
            _dbContext.Claims.Update(claim);
            await _dbContext.SaveEfContextChanges("");
            return claim;
        }
    }
}
