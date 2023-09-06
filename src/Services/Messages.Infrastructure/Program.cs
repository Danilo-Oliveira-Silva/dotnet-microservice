namespace Messages.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using Messages.Infrastructure.Models;
using Messages.Infrastructure.Repository;



public class Persistance
{
    private static AutoResetEvent waitHandle = new AutoResetEvent(false);

    public static void Main(string[] args)
    {
        MessageRepository repository = new MessageRepository();

    Inicio:
        try
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "message",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

            Console.WriteLine("Waiting for new messages");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Message userInput = JsonConvert.DeserializeObject<Message>(message)!;
                Console.WriteLine($" [x] Received {message}");
                repository.Create(userInput);


            };
            channel.BasicConsume(queue: "message",
                    autoAck: true,
                    consumer: consumer);

            Console.CancelKeyPress += (o, e) =>
            {
                Console.WriteLine("Closing...");
                waitHandle.Set();
            };

            // Aguarda que o evento CancelKeyPress ocorra
            waitHandle.WaitOne();
        }
        catch (Exception connectionException)
        {
            Console.WriteLine("Error on connect to rabbitmq");
            Console.WriteLine("Trying again in 5 seconds");
            Console.WriteLine(connectionException.ToString());
            Thread.Sleep(5000);
            goto Inicio;
        }
    }
}