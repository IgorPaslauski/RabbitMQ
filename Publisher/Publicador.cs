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
        
        var properties = model.CreateBasicProperties();
        properties.Persistent = true;
        
        var message = "Hello, World!";
        var body = Encoding.UTF8.GetBytes(message);
        
        model.BasicPublish("exchange-name", "routing-key", properties, body);

        while (true)
        {
            Console.WriteLine("Press [enter] to publish a message or any other key to exit.");
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            
            model.BasicPublish("exchange-name", "routing-key", properties, body);
        }
    }
    
    public ConnectionFactory CreateConnectionFactory()
    {
        return new ConnectionFactory() { HostName = "localhost" };
    }
    
    public IConnection CreateConnection(ConnectionFactory connectionFactory)
    {
        return connectionFactory.CreateConnection();
    }
    
    public IModel CreateModel(IConnection connection)
    {
        return connection.CreateModel();
    }
}