//using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;
using MongoDB.Driver;

using System.Diagnostics.CodeAnalysis;
using ClaimsMS.Core.RabbitMQ;
using ClaimsMS.Common.Dtos.Resolution.Response;
using ClaimsMS.Common.Dtos.Claim.Response;

namespace ClaimsMS.Infrastructure.RabbitMQ.Consumer
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQConsumer: IRabbitMQConsumer
    {
        private readonly IConnectionRabbbitMQ _rabbitMQConnection;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<GetResolutionDto> _collection;
        private readonly IMongoCollection<GetClaimDto> _collectionC;
        private readonly IMongoCollection<GetClaimDeliveryDto> _collectionD;
        public RabbitMQConsumer(IConnectionRabbbitMQ rabbitMQConnection, IMongoCollection<GetResolutionDto> collection, IMongoCollection<GetClaimDto> collectionC, IMongoCollection<GetClaimDeliveryDto> collectionD)
        {
            _rabbitMQConnection = rabbitMQConnection;

            //🔹 Conexión a MongoDB Atlas
            _mongoClient = new MongoClient("mongodb+srv://yadefreitas19:08092001@cluster0.owy2d.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
            _database = _mongoClient.GetDatabase("ClaimMs");
            _collection = collection;
            _collectionC = collectionC;
            _collectionD = collectionD;

        }
        public RabbitMQConsumer() { }

        public async Task ConsumeMessagesAsync(string queueName)
        {
            var channel = _rabbitMQConnection.GetChannel();
            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Mensaje recibido: {message}");

                try
                {
                    var eventMessageD = JsonConvert.DeserializeObject<EventMessage<GetResolutionDto>>(message);
                    var eventMessageC = JsonConvert.DeserializeObject<EventMessage<GetClaimDto>>(message);
                    var eventMessageCD = JsonConvert.DeserializeObject<EventMessage<GetClaimDeliveryDto>>(message);
                    if (eventMessageD?.EventType == "RESOLUTION_CREATED")
                    {
                        await _collection.InsertOneAsync(eventMessageD.Data);
                        Console.WriteLine($"Resolucion insertado en MongoDB: {JsonConvert.SerializeObject(eventMessageD.Data)}");
                    }
                    else if (eventMessageC?.EventType == "CLAIM_UPDATED")
                    {
                        var filter = Builders<GetClaimDto>.Filter.Eq(p => p.ClaimId, eventMessageC.Data.ClaimId);
                        var update = Builders<GetClaimDto>.Update
                            .Set(p => p.ClaimDescription, eventMessageC.Data.ClaimDescription)
                            .Set(p => p.StatusClaim, eventMessageC.Data.StatusClaim)
                            .Set(p => p.ClaimEvidence, eventMessageC.Data.ClaimEvidence)
                            .Set(p => p.ClaimAuctionId, eventMessageC.Data.ClaimAuctionId)
                            .Set(p => p.ClaimReason, eventMessageC.Data.ClaimReason);



                        await _collectionC.UpdateOneAsync(filter, update);
                        Console.WriteLine($"Reclamo actualizado en MongoDB: {JsonConvert.SerializeObject(eventMessageC.Data)}");
                    }
                    else if (eventMessageC?.EventType == "CLAIM_CREATED")
                    {
                        await _collectionC.InsertOneAsync(eventMessageC.Data);
                        Console.WriteLine($"Reclamo insertado en MongoDB: {JsonConvert.SerializeObject(eventMessageC.Data)}");

                       
                    }
                    else if (eventMessageCD?.EventType == "CLAIMDELIVERY_UPDATED")
                    {
                        var filter = Builders<GetClaimDeliveryDto>.Filter.Eq(p => p.ClaimId, eventMessageCD.Data.ClaimId);
                        var update = Builders<GetClaimDeliveryDto>.Update
                            .Set(p => p.ClaimDescription, eventMessageCD.Data.ClaimDescription)
                            .Set(p => p.StatusClaim, eventMessageCD.Data.StatusClaim)
                            .Set(p => p.ClaimEvidence, eventMessageCD.Data.ClaimEvidence)
                            .Set(p => p.ClaimDeliveryId, eventMessageCD.Data.ClaimDeliveryId)
                            .Set(p => p.ClaimReason, eventMessageCD.Data.ClaimReason);



                        await _collectionD.UpdateOneAsync(filter, update);
                        Console.WriteLine($"Reclamo actualizado en MongoDB: {JsonConvert.SerializeObject(eventMessageCD.Data)}");
                    }
                    else if (eventMessageCD?.EventType == "CLAIMDELIVERY_CREATED")
                    {
                        await _collectionD.InsertOneAsync(eventMessageCD.Data);
                        Console.WriteLine($"Reclamo insertado en MongoDB: {JsonConvert.SerializeObject(eventMessageCD.Data)}");


                    }
                    await Task.Run(() => channel.BasicAckAsync(ea.DeliveryTag, false));
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"Error procesando el mensaje: {ex.Message}");
                }
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
            Console.WriteLine("Consumidor de RabbitMQ escuchando mensajes...");
        }
    }
}