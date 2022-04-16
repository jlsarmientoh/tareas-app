using Infrastructure.Interface.Messaging;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Infrastructure.Messaging
{
    public class RabbitMQFactoryCreator : IMessagingFactory
    {
        public IConnectionFactory GetRabitMQFactory(string uri = "amqp://localhost")
        {
            return new ConnectionFactory()
            {
                HostName = uri
            };
        }

        public IConnectionFactory GetRabitMQFactory(IConfiguration configuration)
        {
            return new ConnectionFactory()
            {
                HostName = configuration.GetValue<string>("MQServer")
            };
        }
    }
}