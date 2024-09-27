using AppMoney.Model.RabbitMQ.Connection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace AppMoney.Model.RabbitMQ.Publisher
{
    /// <summary>
    /// Publisher service class to publish message
    /// </summary>
    public class RabbitMQPublisher : PublisherBase, IRabbitMQPublisher
    {
        private readonly IRabbitMqConnection _connection;
        private readonly ILogger<RabbitMQPublisher> _logger;

        public RabbitMQPublisher(IRabbitMqConnection connection,
            ILogger<RabbitMQPublisher> logger):base(connection, logger) 
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishMessageAsync<T>(T message, string queueName)
            => await BaseInvokeAsync(async (channel) =>
        {
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageJson);

            await Task.Run(() => channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body));
        });
    }
}
