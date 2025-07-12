using AutoMapper;
using ClaimsMS.Core.Database;
using ClaimsMS.Core.Repositories.Claims;
using ClaimsMS.Domain.Entities.Claims;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Infrastructure.Repositories.Claims
{
    public class ClaimDeliveryRepository : IClaimDeliveryRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper; // Agregar el Mapper

        public ClaimDeliveryRepository(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }
        public async Task AddAsync(ClaimDelivery claim)
        {
            await _dbContext.ClaimDeliveries.AddAsync(claim);
            await _dbContext.SaveEfContextChanges("");
        }
        public async Task<ClaimDelivery?> UpdateAsync(ClaimDelivery claim)
        {
            _dbContext.ClaimDeliveries.Update(claim);
            await _dbContext.SaveEfContextChanges("");
            return claim;
        }
    }

}
