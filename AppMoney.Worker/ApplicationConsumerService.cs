using AppModey.Database.UnitOfWork;
using AppMoney.Database.Entities;
using AppMoney.Model.Applications.Commands.CreateApplicationCommand;
using AppMoney.Model.RabbitMQ;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ApplicationWorker
{
    public class ApplicationConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ApplicationConsumerService> _logger;
        private readonly IUnitOfWork _dbContext;
        private readonly INotifyCosumer _notifyCosumer;
        private readonly IMapper _mapper;
        private readonly IOptions<RabbitMQOption> _rabbitMqSetting;
        private IConnection _connection;
        private IModel _channel;

        public ApplicationConsumerService(IServiceProvider serviceProvider,
            ILogger<ApplicationConsumerService> logger,
            IUnitOfWork dbContext,
            IOptions<RabbitMQOption> rabbitMqSetting,
            INotifyCosumer notifyCosumer,
            IMapper mapper)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _rabbitMqSetting = rabbitMqSetting ?? throw new ArgumentNullException(nameof(rabbitMqSetting));
            _notifyCosumer = notifyCosumer ?? throw new ArgumentNullException(nameof(notifyCosumer));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.Value.HostName,
                UserName = _rabbitMqSetting.Value.UserName,
                Password = _rabbitMqSetting.Value.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            StartConsuming(RabbitMQQueues.RequestQueue, stoppingToken);
            await Task.CompletedTask;
        }

        private void StartConsuming(string queueName, CancellationToken cancellationToken)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                bool processedSuccessfully = false;
                try
                {
                    processedSuccessfully = await ProcessMessageAsync(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while processing message from queue {queueName}: {ex}");
                }

                if (processedSuccessfully)
                {
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                else
                {
                    _channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                }
            };
            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        private async Task<bool> ProcessMessageAsync(string message)
        {
            bool processedSuccessfully = false;
            using (var scope = _serviceProvider.CreateScope())
            {
                var application = JsonConvert.DeserializeObject<CreateApplicationCommand>(message);
                if (application != null)
                {
                    var result = await _dbContext.Applications
                        .AddAsync(_mapper.Map<Application>(application));

                    processedSuccessfully = true;
                    _notifyCosumer.Notify(result);
                }
            }

            return processedSuccessfully;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
