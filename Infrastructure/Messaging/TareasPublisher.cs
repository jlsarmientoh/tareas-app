using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using Javeriana.Core.Interfaces.Messaging;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Messaging
{
    public class TareasPublisher : IPublisher
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;

        public TareasPublisher(IConfiguration configuration)
        {
            _connectionFactory = new ConnectionFactory() 
            { 
                HostName = configuration.GetValue<string>("MQServer") 
            };
            _queueName = configuration.GetValue<string>("queueName");
        }

        public void DistribuirMensaje(string mensaje)
        {
            
        }

        public void PublicarMensaje(string mensaje)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: _queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                var body = Encoding.UTF8.GetBytes(mensaje);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "task_queue",
                    basicProperties: properties,
                    body: body);

            }
        }
    }
}
