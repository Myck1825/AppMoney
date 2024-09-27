using AppMoney.Respose.ApiResponse;

namespace AppMoney.Model.RabbitMQ.Publisher
{
    public interface IRabbitMQPublisher
    {
        Task PublishMessageAsync<T>(T message, string queueName);
    }
}
