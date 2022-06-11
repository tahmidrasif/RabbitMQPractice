using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQProducer
{
    public class QueueProducer
    {
        public  void Publish(IModel model)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare("demo-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var message = new { Name = "Producer", Message = "" };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            channel.BasicPublish("", "demo-queue", null, body);
        }
    }
}
