using AppMoney.Respose;

namespace AppMoney.Model.RabbitMQ
{
    public class NotifyCosumer : INotifyCosumer
    {
        private TaskCompletionSource<DbResult<Guid>> _taskCompletionSource = default!;

        public NotifyCosumer() { }

        public void SetNotifyFromPublisher(TaskCompletionSource<DbResult<Guid>> taskCompletionSource) 
        {
            _taskCompletionSource = taskCompletionSource;
        }

        public void Notify(DbResult<Guid> response)
        {
            _taskCompletionSource?.TrySetResult(response);
        }
    }
}
