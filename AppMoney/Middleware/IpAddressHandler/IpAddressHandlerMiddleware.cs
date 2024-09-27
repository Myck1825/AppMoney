using AppMoney.Database.Constants;
using AppMoney.Model.Applications.Commands.CreateApplicationCommand;
using System.Net;

namespace AppMoney.Middleware.IpAddressHandler
{
    public class IpAddressHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpAddressHandlerMiddleware> _logger;

        public IpAddressHandlerMiddleware(RequestDelegate next, ILogger<IpAddressHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                IPAddress ipAddress = context.Connection.RemoteIpAddress?.MapToIPv4()!;

                if (context.Request.Headers.ContainsKey(Constants.IpAddressHandlerConstants.XFORWARDEDFOR))
                {
                    var forwardedFor = context.Request.Headers[Constants.IpAddressHandlerConstants.XFORWARDEDFOR].ToString();

                    ipAddress = IPAddress.Parse(forwardedFor.Split(',')[0]);
                }

                context.Items[nameof(CreateApplicationCommand.IpAddress)] = ipAddress?.ToString() ?? Constants.IpAddressHandlerConstants.UNKNOWNIP;

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving client IP address.");
                context.Items[nameof(CreateApplicationCommand.IpAddress)] = "Error retrieving IP";

                throw;
            }
        }
    }
}
