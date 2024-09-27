using AppMoney.Respose;

namespace AppMoney.Model.RabbitMQ
{
    public interface INotifyCosumer
    {
        void SetNotifyFromPublisher(TaskCompletionSource<DbResult<Guid>> taskCompletionSource);
        void Notify(DbResult<Guid> response);
    }
}
