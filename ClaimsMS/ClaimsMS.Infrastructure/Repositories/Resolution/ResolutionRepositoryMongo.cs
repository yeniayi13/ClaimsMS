using AutoMapper;
using ClaimsMS.Common.Dtos.Resolution.Response;
using ClaimsMS.Core.Repositories.Resolutions;
using ClaimsMS.Domain.Entities.Claims;
using ClaimsMS.Domain.Entities.Resolutions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Infrastructure.Repositories.Resolution
{
    public class ResolutionRepositoryMongo : IResolutionRepositoryMongo
    {
        private readonly IMongoCollection<ResolutionEntity> _collection;
        private readonly IMapper _mapper; // Agregar el Mapper

        public ResolutionRepositoryMongo(IMongoCollection<ResolutionEntity> collection, IMapper mapper)
        {

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));// Inyectar el Mapper
        }

        public async Task<List<ResolutionEntity?>> GetAllByFiltredResolutionAsync(Guid? claimId, Guid? id = null)
        {
            Console.WriteLine("Ejecutando GetAllByFiltredResolutionAsync...");

            var filters = new List<FilterDefinition<ResolutionEntity>>();

            if (id.HasValue && id != Guid.Empty)
            {
                Console.WriteLine($"Filtrando por reclamación: {id}");
                filters.Add(Builders<ResolutionEntity>.Filter.Eq("ResolutionId", id));
            }

            if (claimId.HasValue && claimId != Guid.Empty)
            {
                Console.WriteLine($"Filtrando por reclamación: {claimId}");
                filters.Add(Builders<ResolutionEntity>.Filter.Eq("ClaimId", claimId));
            }


            var filter = filters.Count > 0
                ? Builders<ResolutionEntity>.Filter.And(filters)
                : Builders<ResolutionEntity>.Filter.Empty;

            var projection = Builders<ResolutionEntity>.Projection.Exclude("_id");

            var resolutionDtos = await _collection
                .Find(filter)
                .Project<GetResolutionDto>(projection)
                .ToListAsync()
                .ConfigureAwait(false);

            if (resolutionDtos == null || resolutionDtos.Count == 0)
            {
                Console.WriteLine("No se encontraron resoluciones con los filtros proporcionados.");
                return new List<ResolutionEntity>();
            }

            var resolutionEntities = _mapper.Map<List<ResolutionEntity>>(resolutionDtos);
            return resolutionEntities;
        }
    }
}
