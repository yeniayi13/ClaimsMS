using ClaimsMS.Core.Database;
using ClaimsMS.Domain.Entities.Claims;
using ClaimsMS.Domain.Entities.Resolutions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClaimsMS.Infrastructure.Database.Context.Mongo
{

    [ExcludeFromCodeCoverage]
    public class ApplicationDbContextMongo : IApplicationDbContextMongo
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _client;
        private IClientSessionHandle _session;
        private string connectionString;

        public ApplicationDbContextMongo(string connectionString, string databaseName)
        {
            try
            {
                _client = new MongoClient(connectionString);
                _database = _client.GetDatabase(databaseName);
                ConfigureCollections();
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error initializing database context: {e.Message}");
            }
        }
       
        public ApplicationDbContextMongo(string connectionString)
        {
            this.connectionString = connectionString;
            _client = new MongoClient(connectionString);
        }

        public IMongoDatabase Database => _database;
        public IMongoCollection<ResolutionEntity> Resolutions => _database.GetCollection<ResolutionEntity>("Resolutions");
        public IMongoCollection<ClaimEntity> Claims => _database.GetCollection<ClaimEntity>("Claims");

        public IClientSessionHandle BeginTransaction()
        {
            _session = _client.StartSession();
            _session.StartTransaction();
            return _session;
        }

        public void CommitTransaction()
        {
            _session.CommitTransaction();
            Dispose();
        }

        public void AbortTransaction()
        {
            _session.AbortTransaction();
            Dispose();
        }

        public void Dispose()
        {
            _session?.Dispose();
        }

        private void ConfigureCollections()
        {
            try
            {
                var collectionNames = new[]
                {
                    "Resolutions",
                    "Claims"
                };

                var existingCollections = new HashSet<string>(_database.ListCollectionNames().ToEnumerable());
                Console.WriteLine("Existing collections: " + string.Join(", ", existingCollections));

                foreach (var collectionName in collectionNames)
                {
                    if (!existingCollections.Contains(collectionName))
                    {
                        _database.CreateCollection(collectionName);
                        Console.WriteLine($"Collection '{collectionName}' created.");
                    }
                    else
                    {
                        Console.WriteLine($"Collection '{collectionName}' already exists.");
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error configuring collections: {e.Message}");
            }
        }
    }

}
