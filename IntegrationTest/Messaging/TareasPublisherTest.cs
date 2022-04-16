using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.DTO;
using Infrastructure.Interface.Messaging;
using Infrastructure.Messaging;
using Javeriana.Api.DTO;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using Xunit;

namespace IntegrationTests
{
    public class TareasPublisherTest
    {
        private TareasPublisher _tareasPublisher;
        private Mock<RabbitMQ.Client.IConnectionFactory> connectionFactoryMock = new Mock<RabbitMQ.Client.IConnectionFactory>();
        private Mock<ILogger<TareasPublisher>> loggerMock = new Mock<ILogger<TareasPublisher>>();
        private IConfiguration configurationMock = new ConfigurationBuilder().AddInMemoryCollection().Build();
        private Mock<IMessagingFactory> messagingFactoryMock = new Mock<IMessagingFactory>();
        private Mock<IModel> channelMock = new Mock<IModel>();
        private Mock<IConnection> connectionMock = new Mock<IConnection>();
        private Mock<IBasicProperties> basicPropertiesMock = new Mock<IBasicProperties>();

        public TareasPublisherTest()
        {

        }

        [Fact]
        public async Task shouldPublicarMensajeAsync()
        {
            // Arrange
            channelMock.Setup(x => x.CreateBasicProperties()).Returns(basicPropertiesMock.Object);
            channelMock.Setup(x => x.IsClosed).Returns(true);
            channelMock.Setup(x => x.QueueDeclare(It.IsAny<string>(),
                                                   It.IsAny<bool>(),
                                                   It.IsAny<bool>(),
                                                   It.IsAny<bool>(),
                                                   It.IsAny<IDictionary<string, object>>()))
                        .Returns(new QueueDeclareOk("testqueue", 1, 1));
            connectionMock.Setup(x => x.CreateModel()).Returns(channelMock.Object);
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);
            messagingFactoryMock.Setup(m => m.GetRabitMQFactory(It.IsAny<string>())).Returns(connectionFactoryMock.Object);

            _tareasPublisher = new TareasPublisher(messagingFactoryMock.Object, configurationMock, loggerMock.Object);
            var mensaje = new Mensaje
            {
                FechaEnvio = DateTime.Now,
                Tarea = new Tarea
                {
                    Id = 1,
                    Name = "Tarea 1",
                    IsComplete = false
                }
            };

            // Act
            await _tareasPublisher.PublicarMensajeAsync(mensaje);
            // Assert
            messagingFactoryMock.Verify(m => m.GetRabitMQFactory(It.IsAny<string>()), Times.Once);
            connectionFactoryMock.Verify(m => m.CreateConnection(), Times.Once);
            connectionMock.Verify(m => m.CreateModel(), Times.Exactly(2));
            channelMock.Verify(m => m.QueueDeclare(It.IsAny<string>(),
                                                   It.IsAny<bool>(),
                                                   It.IsAny<bool>(),
                                                   It.IsAny<bool>(),
                                                   It.IsAny<IDictionary<string, object>>()), Times.Once);
            channelMock.Verify(m => m.CreateBasicProperties(), Times.Once);
        }

        [Fact]
        public async Task shouldDistribuirMensajeAsync()
        {
            // Arrange
            channelMock.Setup(x => x.CreateBasicProperties()).Returns(basicPropertiesMock.Object);
            channelMock.Setup(x => x.IsClosed).Returns(true);
            channelMock.Setup(x => x.QueueDeclare(It.IsAny<string>(),
                                                   It.IsAny<bool>(),
                                                   It.IsAny<bool>(),
                                                   It.IsAny<bool>(),
                                                   It.IsAny<IDictionary<string, object>>()))
                        .Returns(new QueueDeclareOk("testqueue", 1, 1));
            connectionMock.Setup(x => x.CreateModel()).Returns(channelMock.Object);
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);
            messagingFactoryMock.Setup(m => m.GetRabitMQFactory(It.IsAny<string>())).Returns(connectionFactoryMock.Object);

            _tareasPublisher = new TareasPublisher(messagingFactoryMock.Object, configurationMock, loggerMock.Object);
            var mensaje = new Mensaje
            {
                FechaEnvio = DateTime.Now,
                Tarea = new Tarea
                {
                    Id = 1,
                    Name = "Tarea 1",
                    IsComplete = false
                }
            };

            // Act
            await _tareasPublisher.DistribuirMensajeAsync(mensaje);
            // Assert
            messagingFactoryMock.Verify(m => m.GetRabitMQFactory(It.IsAny<string>()), Times.Once);
            connectionFactoryMock.Verify(m => m.CreateConnection(), Times.Once);
            connectionMock.Verify(m => m.CreateModel(), Times.Exactly(2));
            channelMock.Verify(m => m.ExchangeDeclare(It.IsAny<string>(),
                                                   It.IsAny<string>(),
                                                   It.IsAny<bool>(),
                                                   It.IsAny<bool>(),
                                                   It.IsAny<IDictionary<string, object>>()), Times.Once);
            channelMock.Verify(m => m.CreateBasicProperties(), Times.Never);
        }
    }
}