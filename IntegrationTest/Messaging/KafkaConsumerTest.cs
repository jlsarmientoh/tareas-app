using System;
using ApplicationCore.DTO;
using Infrastructure.Messaging;
using Javeriana.Api.DTO;
using Xunit;

namespace IntegrationTests
{
    public class KafkaConsumerTest
    {
        private readonly KafkaConsumer kakkaConsumer = new KafkaConsumer();
        [Fact]
        public void shouldThrowExceptionOnProcesarMensaje()
        {
            // Assert
            Assert.ThrowsAsync<NotImplementedException>(() => kakkaConsumer.ProcesarMensaje(new Mensaje
            {
                FechaEnvio = DateTime.Now,
                Tarea = null
            }));
        }
    }
}