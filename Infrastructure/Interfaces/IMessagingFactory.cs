using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Infrastructure.Interface.Messaging
{
    public interface IMessagingFactory
    {
        IConnectionFactory GetRabitMQFactory(string uri);
        IConnectionFactory GetRabitMQFactory(IConfiguration configuration);
    }
}
