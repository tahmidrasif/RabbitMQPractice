using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQProducerExchange
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Producer Exchange Started...");
            var ttl = new Dictionary<string, object>()
            {
                {"x-message-ttl",3000 }
            };
            while (true)
            {
                Console.WriteLine("Waiting for new message");
                var txt = Console.ReadLine();
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri("amqp://guest:guest@localhost:5672")
                };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                channel.ExchangeDeclare("demo-direct-exchange", ExchangeType.Direct,arguments:ttl);

                var message = new { Name = "Producer", Message = txt };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish("demo-direct-exchange", "demo.init", null, body);

            }
        }
    }
}
