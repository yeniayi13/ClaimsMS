using AutoMapper;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Core.Repositories.Claims;
using ClaimsMS.Domain.Entities.Claims;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Infrastructure.Repositories.Claims
{
    public class ClaimRepositoryMongo : IClaimRepositoryMongo
    {
        private readonly IMongoCollection<ClaimEntity> _collection;
        private readonly IMapper _mapper; // Agregar el Mapper

        public ClaimRepositoryMongo(IMongoCollection<ClaimEntity> collection, IMapper mapper)
        {

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));// Inyectar el Mapper
        }

        public async Task<ClaimEntity?> GetByIdAsync(Guid id)
        {
            Console.WriteLine($"Buscando reclamo con ID: {id} ");

            var filters = Builders<ClaimEntity>.Filter.And(
                Builders<ClaimEntity>.Filter.Eq("ClaimId", id)
               
            );

            var projection = Builders<ClaimEntity>.Projection.Exclude("_id");

            var claimDto = await _collection
                .Find(filters)
                .Project<GetClaimDto>(projection) // Convertir el resultado al DTO
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (claimDto == null)
            {
                Console.WriteLine("Producto no encontrado para este usuario.");
                return null;
            }

            var productEntity = _mapper.Map<ClaimEntity>(claimDto);
            return productEntity;
        }

        public async Task<List<ClaimEntity?>> GetByStatusClaimsAsync(Guid? userId, Guid? auctionId = null, string? status = null)
        {
            Console.WriteLine("Ejecutando GetByStatusClaimsAsync...");

            var filters = new List<FilterDefinition<ClaimEntity>>();

            if (userId.HasValue && userId != Guid.Empty)
            {
                Console.WriteLine($"Filtrando por usuario: {userId}");
                filters.Add(Builders<ClaimEntity>.Filter.Eq("UserId", userId));
            }

            if (auctionId.HasValue && auctionId != Guid.Empty)
            {
                Console.WriteLine($"Filtrando por subasta: {auctionId}");
                filters.Add(Builders<ClaimEntity>.Filter.Eq("AuctionId", auctionId));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                Console.WriteLine($"Filtrando por estado: {status}");
                filters.Add(Builders<ClaimEntity>.Filter.Eq("Status", status));
            }

            var filter = filters.Count > 0
                ? Builders<ClaimEntity>.Filter.And(filters)
                : Builders<ClaimEntity>.Filter.Empty;

            var projection = Builders<ClaimEntity>.Projection.Exclude("_id");

            var claimDtos = await _collection
                .Find(filter)
                .Project<GetClaimDto>(projection)
                .ToListAsync()
                .ConfigureAwait(false);

            if (claimDtos == null || claimDtos.Count == 0)
            {
                Console.WriteLine("No se encontraron reclamaciones con los filtros proporcionados.");
                return new List<ClaimEntity>();
            }

            var claimEntities = _mapper.Map<List<ClaimEntity>>(claimDtos);
            return claimEntities;
        }
    }
}
