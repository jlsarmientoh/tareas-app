using ApplicationCore.DTO;
using Javeriana.Core.Interfaces.Messaging;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public class KafkaConsumer : BackgroundService, IConsumer
    {
        public Task ProcesarMensaje(Mensaje mensaje)
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
