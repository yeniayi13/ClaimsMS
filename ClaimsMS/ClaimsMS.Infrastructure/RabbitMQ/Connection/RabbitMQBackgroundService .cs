
using ClaimsMS.Core.RabbitMQ;
using Microsoft.Extensions.Hosting;

namespace ClaimsMS.Infrastructure.RabbitMQ.Connection
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly IRabbitMQConsumer _rabbitMQConsumer;

        public RabbitMQBackgroundService(IRabbitMQConsumer rabbitMQConsumer)
        {
            _rabbitMQConsumer = rabbitMQConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine(" Esperando la inicialización de RabbitMQ...");

            await Task.Delay(3000); // Pequeño retraso para asegurar la inicialización
            var queues = new List<string> { "resolutionQueue", "claimQueue" };


            foreach (var queue in queues)
            {
                _ = Task.Run(() => _rabbitMQConsumer.ConsumeMessagesAsync(queue), stoppingToken);
            }
        }
    }
}
