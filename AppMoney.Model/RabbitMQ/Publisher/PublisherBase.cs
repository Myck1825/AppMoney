using AppMoney.Model.RabbitMQ.Connection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace AppMoney.Model.RabbitMQ.Publisher
{
    public abstract class PublisherBase
    {
        private readonly IRabbitMqConnection _connection;
        private readonly ILogger<PublisherBase> _logger;

        public PublisherBase(IRabbitMqConnection connection,
            ILogger<PublisherBase> logger)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task BaseInvokeAsync(Func<IModel, Task> func)
        {
            try
            {
                using IConnection connection = _connection.CreateConnection();
                using var channel = connection.CreateModel();

                await func(channel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
