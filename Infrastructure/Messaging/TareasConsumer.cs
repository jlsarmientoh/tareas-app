using ApplicationCore.DTO;
using Javeriana.Core.Interfaces.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public class TareasConsumer : BackgroundService, IConsumer
    {
        private readonly ILogger<TareasConsumer> _logger;
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;
        private IModel _channel;
        private IConnection _connection;

        public TareasConsumer(IConfiguration configuration, ILogger<TareasConsumer> logger)
        {
            _logger = logger;
            _connectionFactory = new ConnectionFactory()
            {
                HostName = configuration.GetValue<string>("MQServer")
            };
            _queueName = configuration.GetValue<string>("queueName");
            init();
        }

        private void init()
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                    queue: _queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        }


        public async Task ProcesarMensaje(Mensaje mensaje)
        {
            _logger.LogInformation($"[Mensaje recibido] - [Fecha:{mensaje.FechaEnvio}] - [Tarea: {mensaje.Tarea.Id} {mensaje.Tarea.Name} {mensaje.Tarea.IsComplete}]");

            string fileName = "tareas.csv";
            string registro = $"{mensaje.FechaEnvio},{mensaje.Tarea.Id},{mensaje.Tarea.Name},{mensaje.Tarea.IsComplete}";

            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                await writer.WriteLineAsync(registro);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, enventArgs) =>
            {
                string content = Encoding.UTF8.GetString(enventArgs.Body.ToArray());
                Mensaje mensaje = JsonConvert.DeserializeObject<Mensaje>(content);

                await ProcesarMensaje(mensaje);

                _channel.BasicAck(enventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
