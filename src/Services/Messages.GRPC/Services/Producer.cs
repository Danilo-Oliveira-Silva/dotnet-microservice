using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace Messages.GRPC.Services;

public class Producer : IProducer
{
    public void SendMessage<T> (T message)
    {
        var _factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            {

                channel.QueueDeclare(queue: "message",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

                var stringMessage = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(stringMessage);

                channel.BasicPublish(exchange: string.Empty,
                    routingKey: "message",
                    basicProperties: null,
                    body: body);
            }
    }
}