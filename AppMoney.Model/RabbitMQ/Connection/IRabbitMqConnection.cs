using RabbitMQ.Client;

namespace AppMoney.Model.RabbitMQ.Connection
{
    public interface IRabbitMqConnection
    {
        IConnection CreateConnection();
    }
}
