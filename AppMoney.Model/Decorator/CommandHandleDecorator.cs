using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Diagnostics;
using AppMoney.Respose.ApiResponse;

namespace AppMoney.Model.Decorator
{
    public class CommandHandleDecorator<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : notnull, MediatR.IRequest<TResponse>
    {
        private readonly IRequestHandler<TCommand, TResponse> _commandHandler;
        private readonly ILogger<CommandHandleDecorator<TCommand, TResponse>> _logger;

        public CommandHandleDecorator(IRequestHandler<TCommand, TResponse> commandHandler,
            ILogger<CommandHandleDecorator<TCommand, TResponse>> logger)
        {
            _commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken)
        {
            string commandName = typeof(TCommand).Name;
            string uniqueId = Guid.NewGuid().ToString();
            string commandJson = JsonSerializer.Serialize(request);
            try
            {
                _logger.LogInformation($"Begin command id: {uniqueId}, command name: {commandName}, " +
                    $"command json {commandJson}");

                var timer = new Stopwatch();
                timer.Start();

                var response = await _commandHandler.Handle(request, cancellationToken);

                timer.Stop();
                _logger.LogInformation($"End Request Id:{uniqueId}, request name:{commandName}, total request time:{timer.ElapsedMilliseconds}ms");
                return response;
            }
            catch(Exception ex) 
            {
                _logger.LogError($"command id: {uniqueId}, command name: {commandName}, command json {commandJson}" +
                    $"exception: {ex}");
                throw;
            }
        }
    }
}