using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.DTO;
using Infrastructure.Interface.Messaging;
using Infrastructure.Messaging;
using Javeriana.Api.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using Xunit;

namespace IntegrationTests
{
    public class TareasConsumerTest{

        private TareasConsumer _tareasConsumer;
        private Mock<IConnectionFactory> connectionFactoryMock = new Mock<IConnectionFactory>();
        private Mock<ILogger<TareasConsumer>> loggerMock = new Mock<ILogger<TareasConsumer>>();
        private IConfiguration configurationMock = new ConfigurationBuilder().AddInMemoryCollection().Build();
        private Mock<IMessagingFactory> messagingFactoryMock = new Mock<IMessagingFactory>();

        private Mock<IModel> channelMock = new Mock<IModel>();
        private Mock<IConnection> connectionMock = new Mock<IConnection>();

        public TareasConsumerTest()
        {
            
        }

        [Fact]
        public async Task shouldProcesarMensajeAsync()
        {
            // Arrange
            channelMock.Setup(x => x.QueueDeclare(It.IsAny<string>(),
                                                  It.IsAny<bool>(),
                                                  It.IsAny<bool>(),
                                                  It.IsAny<bool>(),
                                                  It.IsAny<IDictionary<string, object>>()))
                        .Returns(new QueueDeclareOk("testqueue", 1, 1));
            connectionMock.Setup(x => x.CreateModel()).Returns(channelMock.Object);
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);
            messagingFactoryMock.Setup(m => m.GetRabitMQFactory(It.IsAny<string>())).Returns(connectionFactoryMock.Object);

            _tareasConsumer = new TareasConsumer(messagingFactoryMock.Object, configurationMock, loggerMock.Object);
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
            await _tareasConsumer.ProcesarMensaje(mensaje);
            // Assert
            messagingFactoryMock.Verify(m => m.GetRabitMQFactory(It.IsAny<string>()), Times.Once);
            connectionFactoryMock.Verify(m => m.CreateConnection(), Times.Once);
            connectionMock.Verify(m => m.CreateModel(), Times.Once);
        }
    }
}