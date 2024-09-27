using AppMoney.Respose.ApiResponse;
using AppMoney.Respose.CustomException;
using AppMoney.Respose.Enums;
using Microsoft.AspNetCore.Diagnostics;

namespace AppMoney.ExceptionHandlers.Middleware
{
    public class AlreadyExceptionHandler(ILogger<AlreadyExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is AlreadyExistEcxeption alreadyExistException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                logger.LogError($"Conflict exception: {exception}");
                var response = new ServerResponse<object>()
                {
                    Message = exception.Message,
                    IsSuccess = false,
                    Code = Code.Conflict
                };
                await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                return true;
            }

            return false;
        }
    }
}
