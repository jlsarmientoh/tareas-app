using ApplicationCore.DTO;
using Infrastructure.Interface.Messaging;
using Javeriana.Core.Interfaces.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public class LogConsumer : BackgroundService, IConsumer
    {
        private readonly ILogger<LogConsumer> _logger;
        private readonly IConnectionFactory _connectionFactory;
        private readonly string _exchangeName;
        private string _queueName;
        private IModel _channel;
        private IConnection _connection;

        public LogConsumer(IMessagingFactory messagingFactory, IConfiguration configuration, ILogger<LogConsumer> logger)
        {
            _logger = logger;
            _connectionFactory = messagingFactory.GetRabitMQFactory(configuration.GetValue<string>("MQServer"));
            _exchangeName = configuration.GetValue<string>("exchangeName");
            Init();
        }

        private void Init()
        {
            try 
            {
                _connection = _connectionFactory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(
                    exchange: _exchangeName,
                    type: ExchangeType.Fanout
                    );

                _queueName = _channel.QueueDeclare().QueueName;

                _channel.QueueBind(
                    queue: _queueName,
                    exchange: _exchangeName,
                    routingKey: ""
                    );
            }
            catch (BrokerUnreachableException ex)
            {
                _logger.LogError(ex, $"No se puede conectar a RabbitMQ : {ex.Message}");
            }
            
        }

        public Task ProcesarMensaje(Mensaje mensaje)
        {
            return Task.Run(() => { 
                _logger.LogInformation($"[Mensaje recibido] - [Fecha:{mensaje.FechaEnvio}] - [Tarea: {mensaje.Tarea.Id} {mensaje.Tarea.Name} {mensaje.Tarea.IsComplete}]");
            });
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
            };

            _channel?.BasicConsume(_queueName, true, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
