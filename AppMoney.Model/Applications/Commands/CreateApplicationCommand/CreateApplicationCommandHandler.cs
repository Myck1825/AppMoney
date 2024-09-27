using AppMoney.Model.RabbitMQ;
using AppMoney.Model.RabbitMQ.Publisher;
using AppMoney.Respose;
using AppMoney.Respose.ApiResponse;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppMoney.Model.Applications.Commands.CreateApplicationCommand
{
    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, ServerResponse<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateApplicationCommandHandler> _logger;
        private readonly IRabbitMQPublisher _publisher;
        private readonly INotifyCosumer _notifyCosumer;

        public CreateApplicationCommandHandler(IMapper mapper,
            ILogger<CreateApplicationCommandHandler> logger,
            IRabbitMQPublisher publisher,
            INotifyCosumer notifyCosumer)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _notifyCosumer = notifyCosumer ?? throw new ArgumentNullException(nameof(notifyCosumer));
        }

        public async Task<ServerResponse<Guid>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var taskCompletionSource = new TaskCompletionSource<DbResult<Guid>>();
            _notifyCosumer.SetNotifyFromPublisher(taskCompletionSource);
            await _publisher.PublishMessageAsync(request, RabbitMQQueues.RequestQueue);

            // Wait for the consumer to complete the database operation
            var result = await taskCompletionSource.Task;
            if (!result.IsSuccess)
                throw result.Exception!;

            return _mapper.Map<ServerResponse<Guid>>(result);
        }
    }
}
