using ClaimsMS.Domain.Entities.Resolutions;
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
    public class ResolutionConfigurationMongo
    {
        public static void Configure(IMongoCollection<ResolutionEntity> collection)
        {
            // Crear un índice único en ResolutionId para evitar duplicados
            var indexKeysDefinition = Builders<ResolutionEntity>.IndexKeys.Ascending(c => c.ResolutionId.Value);
            var indexOptions = new CreateIndexOptions { Unique = true };
            var indexModel = new CreateIndexModel<ResolutionEntity>(indexKeysDefinition, indexOptions);
            collection.Indexes.CreateOne(indexModel);

            // Índice en ClaimId para mejorar la búsqueda por reclamo
            indexKeysDefinition = Builders<ResolutionEntity>.IndexKeys.Ascending(c => c.ClaimId.Value);
            collection.Indexes.CreateOne(indexKeysDefinition);

         
        }
    }
}
