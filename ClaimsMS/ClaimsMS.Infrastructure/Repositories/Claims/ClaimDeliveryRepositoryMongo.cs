using AutoMapper;
using ClaimsMS.Common.Dtos.Claim.Response;
using ClaimsMS.Core.Repositories.Claims;
using ClaimsMS.Domain.Entities.Claims;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClaimsMS.Infrastructure.Repositories.Claims
{
    public class ClaimDeliveryRepositoryMongo : IClaimDeliveryRepositoryMongo
    {
        private readonly IMongoCollection<ClaimDelivery> _collection;
        private readonly IMapper _mapper; // Agregar el Mapper

        public ClaimDeliveryRepositoryMongo(IMongoCollection<ClaimDelivery> collection, IMapper mapper)
        {

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));// Inyectar el Mapper
        }

        public async Task<ClaimDelivery?> GetByIdAsync(Guid id)
        {
            Console.WriteLine($"Buscando reclamo con ID: {id} ");

            var filters = Builders<ClaimDelivery>.Filter.And(
                Builders<ClaimDelivery>.Filter.Eq("ClaimId", id)

            );

            var projection = Builders<ClaimDelivery>.Projection.Exclude("_id");

            var claimDto = await _collection
                .Find(filters)
                .Project<GetClaimDeliveryDto>(projection) // Convertir el resultado al DTO
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (claimDto == null)
            {
                Console.WriteLine("Producto no encontrado para este usuario.");
                return null;
            }

            var productEntity = _mapper.Map<ClaimDelivery>(claimDto);
            return productEntity;
        }

        public async Task<List<ClaimDelivery?>> GetByStatusClaimsAsync(Guid? userId, Guid? deliveryId = null, string? status = null)
        {
            Console.WriteLine("Ejecutando GetByStatusClaimsAsync...");

            var filters = new List<FilterDefinition<ClaimDelivery>>();

            if (userId.HasValue && userId != Guid.Empty)
            {
                Console.WriteLine($"Filtrando por usuario: {userId}");
                filters.Add(Builders<ClaimDelivery>.Filter.Eq("ClaimUserId", userId));
            }

            if (deliveryId.HasValue && deliveryId != Guid.Empty)
            {
                Console.WriteLine($"Filtrando por subasta: {deliveryId}");
                filters.Add(Builders<ClaimDelivery>.Filter.Eq("ClaimDeliveryId", deliveryId));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                var regex = new BsonRegularExpression($"^{Regex.Escape(status.Trim())}$", "i"); // 'i' => case-insensitive
                                                                                                // filters.Add(Builders<Bid>.Filter.Regex("Status", regex));
                filters.Add(Builders<ClaimDelivery>.Filter.Eq("StatusClaim", regex));
            }

            var filter = filters.Count > 0
                ? Builders<ClaimDelivery>.Filter.And(filters)
                : Builders<ClaimDelivery>.Filter.Empty;

            var projection = Builders<ClaimDelivery>.Projection.Exclude("_id");

            var claimDtos = await _collection
                .Find(filter)
                .Project<GetClaimDeliveryDto>(projection)
                .ToListAsync()
                .ConfigureAwait(false);

            if (claimDtos == null || claimDtos.Count == 0)
            {
                Console.WriteLine("No se encontraron reclamaciones con los filtros proporcionados.");
                return new List<ClaimDelivery>();
            }

            var claimEntities = _mapper.Map<List<ClaimDelivery>>(claimDtos);
            return claimEntities;
        }

        public async Task<List<ClaimDelivery>> GetAllClaimDelivery()
        {
            var projection = Builders<ClaimDelivery>.Projection.Exclude("_id");
            var bidsDto = await _collection.Find(Builders<ClaimDelivery>.Filter.Empty)
                .Project<GetClaimDeliveryDto>(projection)
                .ToListAsync();

            return _mapper.Map<List<ClaimDelivery>>(bidsDto);


        }
    }
}
