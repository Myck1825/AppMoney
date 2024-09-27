using AppMoney.Respose.ApiResponse;
using AppMoney.Respose.CustomException;
using AppMoney.Respose.Enums;
using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace AppMoney.ExceptionHandlers.Middleware
{
    public class ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            if (exception is ValidationException validationException || exception is RecordIsIncorrectException recordIsIncorrectException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                logger.LogError($"Bad Request exception: {exception}");
                var response = new ServerResponse<object>()
                {
                    Message = exception.Message,
                    IsSuccess = false,
                    Code = Code.BadRequest
                };
                await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                return true;
            }

            return false;
        }
    }
}
