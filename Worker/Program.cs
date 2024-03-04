// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

const string Queue = "message";

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(Queue, durable: true, exclusive: false, autoDelete: false);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, arg) =>
{
    var body = arg.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [X] received {message}");

    channel.BasicAck(deliveryTag: arg.DeliveryTag, multiple: false);

    int dots = message.Split('.').Length - 1;
    Thread.Sleep(dots * 1000);
};

channel.BasicConsume(Queue, autoAck: true, consumer: consumer);

Console.WriteLine("Press [enter] to exit");
Console.ReadLine();