using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQConsumerExchange
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid guid = new Guid();
            Console.WriteLine("Consumer-" + guid + " has been started");
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.ExchangeDeclare("demo-direct-exchange", ExchangeType.Direct);
            channel.QueueDeclare("demo-direct-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind("demo-direct-queue", "demo-direct-exchange", "demo.init");
            channel.BasicQos(0, 10,false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            channel.BasicConsume("demo-direct-queue", autoAck: true, consumer);
            Console.ReadLine();
        }
    }
}
