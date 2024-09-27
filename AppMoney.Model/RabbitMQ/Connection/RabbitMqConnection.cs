using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace AppMoney.Model.RabbitMQ.Connection
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly RabbitMQOption _rabbitMqSetting;

        public RabbitMqConnection(IOptions<RabbitMQOption> rabbitMqSetting)
        {
            _rabbitMqSetting = rabbitMqSetting?.Value ?? throw new ArgumentNullException(nameof(rabbitMqSetting));
        }

        public IConnection CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };

            return factory.CreateConnection();
        }
    }
}
