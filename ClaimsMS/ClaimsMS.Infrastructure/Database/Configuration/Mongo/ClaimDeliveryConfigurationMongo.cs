using ClaimsMS.Domain.Entities.Claims;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Infrastructure.Database.Configuration.Mongo
{
    public class ClaimDeliveryConfigurationMongo
    {
        public static void Configure(IMongoCollection<ClaimDelivery> collection)
        {
            // Índice único en ProductId para evitar duplicados
            var indexKeysDefinition = Builders<ClaimDelivery>.IndexKeys.Ascending(p => p.ClaimId.Value);
            var indexOptions = new CreateIndexOptions { Unique = true };
            var indexModel = new CreateIndexModel<ClaimDelivery>(indexKeysDefinition, indexOptions);
            collection.Indexes.CreateOne(indexModel);

            // Índice en ClaimID para optimizar búsqueda por subasta
            indexKeysDefinition = Builders<ClaimDelivery>.IndexKeys.Ascending(p => p.ClaimDeliveryId.Value);
            collection.Indexes.CreateOne(indexKeysDefinition);

            // Índice en ClaimUserId para optimizar consultas por usuario
            indexKeysDefinition = Builders<ClaimDelivery>.IndexKeys.Ascending(p => p.ClaimUserId.Value);
            collection.Indexes.CreateOne(indexKeysDefinition);

            // Índice en StatusClaim para optimizar consultas por estado de reclamo
            indexKeysDefinition = Builders<ClaimDelivery>.IndexKeys.Ascending(p => p.StatusClaim.ToString());
            collection.Indexes.CreateOne(indexKeysDefinition);


        }
    }
}
