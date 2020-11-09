using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using Javeriana.Core.Interfaces.Messaging;
using Microsoft.Extensions.Configuration;
using ApplicationCore.DTO;
using Newtonsoft.Json;

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

        public void PublicarMensaje(Mensaje mensaje)
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

                string content = JsonConvert.SerializeObject(mensaje);
                var body = Encoding.UTF8.GetBytes(content);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(
                    exchange: "",
                    routingKey: _queueName,
                    basicProperties: properties,
                    body: body);

            }
        }
    }
}
