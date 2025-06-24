using ClaimsMS.Domain.Entities.Claims;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsMS.Infrastructure.Database.Configuration.Mongo
{

    [ExcludeFromCodeCoverage]
    public class ClaimConfigurationMongo
    {
        public static void Configure(IMongoCollection<ClaimEntity> collection)
        {
            // Índice único en ProductId para evitar duplicados
            var indexKeysDefinition = Builders<ClaimEntity>.IndexKeys.Ascending(p => p.ClaimId.Value);
            var indexOptions = new CreateIndexOptions { Unique = true };
            var indexModel = new CreateIndexModel<ClaimEntity>(indexKeysDefinition, indexOptions);
            collection.Indexes.CreateOne(indexModel);

            // Índice en ClaimID para optimizar búsqueda por subasta
            indexKeysDefinition = Builders<ClaimEntity>.IndexKeys.Ascending(p => p.ClaimAuctionId.Value);
            collection.Indexes.CreateOne(indexKeysDefinition);

            // Índice en ClaimUserId para optimizar consultas por usuario
            indexKeysDefinition = Builders<ClaimEntity>.IndexKeys.Ascending(p => p.ClaimUserId.Value);
            collection.Indexes.CreateOne(indexKeysDefinition);

            // Índice en StatusClaim para optimizar consultas por estado de reclamo
            indexKeysDefinition = Builders<ClaimEntity>.IndexKeys.Ascending(p => p.StatusClaim.ToString());
            collection.Indexes.CreateOne(indexKeysDefinition);

         
        }
    }
}
