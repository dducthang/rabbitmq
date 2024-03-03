using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQConsumer
{
    public class Program
    {
        public static void BindQueues(string[] queueNames, string exchange)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange, type: ExchangeType.Fanout);

            foreach (var queueName in queueNames)
            {
                channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind(queueName,
                      exchange: exchange,
                      routingKey: string.Empty);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, args) =>
                {
                    byte[] body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] {queueName} {message}");
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
            }
            Console.ReadKey();

        }

        public static void BindQueue(string queueName, string exchange)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange, type: ExchangeType.Fanout);

            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queueName,
                    exchange: exchange,
                    routingKey: string.Empty);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, args) =>
            {
                byte[] body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] {queueName} {message}");
            };
            channel.BasicConsume(queue: queueName,
                                    autoAck: true,
                                    consumer: consumer);

            Console.ReadKey();

        }
        public static void ConsumeMessage()
        {
            var queueNames = new string[] { "messages", "secret" };
            var queueName = "message";
            var exchange = "logs";
            BindQueues(queueNames, exchange);    
            
        }
        static void Main(string[] args)
        {
            ConsumeMessage();
        }
    }
}