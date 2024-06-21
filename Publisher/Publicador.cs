using System.Text;
using RabbitMQ.Client;

namespace Publisher;

public class Publicador
{
    public void Publicar()
    {
        var connectionFactory = CreateConnectionFactory();
        using var connection = CreateConnection(connectionFactory);
        using var model = CreateModel(connection);
        model.ExchangeDeclare("exchange-name", ExchangeType.Direct);
        model.QueueDeclare("queue-name", durable: true, exclusive: false, autoDelete: false);
        
        while (true)
        {
            Thread.Sleep(1000);
                
            for (var i = 0; i < 10; i++)
            {
                var message = $"Message {i}";

                model.BasicPublish("exchange-name", i % 2 == 0 ? "routing-key" : "routing-key2", null,
                    Encoding.UTF8.GetBytes(message));
            }
            
        }
    }
    private static ConnectionFactory CreateConnectionFactory() => new() { HostName = "localhost" };
    private static IConnection CreateConnection(ConnectionFactory connectionFactory) => connectionFactory.CreateConnection();
    private static IModel CreateModel(IConnection connection) => connection.CreateModel();
}