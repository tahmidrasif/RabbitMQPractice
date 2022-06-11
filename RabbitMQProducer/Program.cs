using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Producer Started...");
            while (true)
            {
                Console.WriteLine("Waiting for new message");
                var txt=Console.ReadLine();
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri("amqp://guest:guest@localhost:5672")
                };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                channel.QueueDeclare("demo-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var message = new { Name = "Producer", Message = txt };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish("", "demo-queue", null, body);

            }

        }
    }
}
