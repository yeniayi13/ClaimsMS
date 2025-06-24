using AutoMapper;
using ClaimsMS.Core.Database;
using ClaimsMS.Core.Repositories.Resolutions;
using ClaimsMS.Domain.Entities.Claims;
using ClaimsMS.Domain.Entities.Resolutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Infrastructure.Repositories.Resolution
{
    public class ResolutionRepository: IResolutionRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper; // Agregar el Mapper

        public ResolutionRepository(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }
        public async Task AddAsync(ResolutionEntity resolution)
        {
            await _dbContext.Resolutions.AddAsync(resolution);
            await _dbContext.SaveEfContextChanges("");
        }
       
    }
}
