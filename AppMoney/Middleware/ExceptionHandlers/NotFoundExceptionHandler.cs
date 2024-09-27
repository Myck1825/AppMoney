using AppMoney.Respose.ApiResponse;
using AppMoney.Respose.CustomException;
using AppMoney.Respose.Enums;
using Microsoft.AspNetCore.Diagnostics;

namespace AppMoney.ExceptionHandlers.Middleware
{
    public class NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is NotFoundException notFoundException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                logger.LogError($"Not Found exception: {exception}");
                var response = new ServerResponse<object>()
                {
                    Message = exception.Message,
                    IsSuccess = false,
                    Code = Code.NotFound
                };
                await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                return true;
            }

            return false;
        }
    }
}
