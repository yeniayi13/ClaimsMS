using Microsoft.EntityFrameworkCore.Metadata;
using ClaimsMS.Core.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMQConnection : IConnectionRabbbitMQ
{
    private IConnection _connection;
    private IChannel _channel;
    private readonly IConnectionFactory _connectionFactory;

    public RabbitMQConnection(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        // 🔹 Usa la instancia inyectada en el constructor
        _connection = await _connectionFactory.CreateConnectionAsync(CancellationToken.None);

        if (_connection == null)
        {
            throw new InvalidOperationException("No se pudo establecer la conexión con RabbitMQ.");
        }

        _channel = await _connection.CreateChannelAsync();

        if (_channel == null)
        {
            throw new InvalidOperationException("No se pudo crear el canal de comunicación con RabbitMQ.");
        }
        var queues = new List<string> { "resolutionQueue", "claimQueue", "claimDeliveryQueue" };


        foreach (var queue in queues)
        {
            await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false);
            Console.WriteLine($" Cola '{queue}' declarada correctamente.");
        }
    }

    public IChannel GetChannel()
    {
        if (_channel == null)
        {
            throw new InvalidOperationException("RabbitMQ aún no está inicializado correctamente.");
        }
        return _channel;
    }

   

}