using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public static class Program
{
    public static void Main()
    {
        var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = connectionFactory.CreateConnection();
        using var model = connection.CreateModel();
        
        model.ExchangeDeclare("exchange-name", ExchangeType.Direct);
        
        var queueName = model.QueueDeclare().QueueName;
        model.QueueBind(queueName, "exchange-name", "routing-key");
        
        var consumer = new EventingBasicConsumer(model);
        consumer.Received += (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received message: {message}");
        };
        
        model.BasicConsume(queueName, true, consumer);
        
        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();
    }
}