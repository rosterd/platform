using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using Rosterd.Domain.Exceptions;

namespace Rosterd.Web.Infra.Middleware
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorMessages = new List<string>();
            if (exception is BaseRosterdException rosterdException)
                errorMessages = rosterdException.Messages;

            var errorDetails = exception switch
            {
                EntityAlreadyExistsException entityAlreadyExistsException => ErrorDetails.GenerateEntityAlreadyExistsError(errorMessages),
                EntityNotFoundException entityNotFoundException => ErrorDetails.Generate404Error(errorMessages),
                BadRequestException badRequestException => ErrorDetails.Generate400Error(errorMessages),
                _ => ErrorDetails.Generate500Error()
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorDetails.StatusCode;

            await context.Response.WriteAsync(errorDetails.ToString());
        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public List<string> Messages { get; set; }
        public override string ToString() => JsonSerializer.Serialize(this);

        public static ErrorDetails Generate500Error() => new ErrorDetails()
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Messages = new List<string> { "An unexpected error has occurred, please contact support."}
        };

        public static ErrorDetails Generate404Error(List<string> messages) => new ErrorDetails()
        {
            StatusCode = (int)HttpStatusCode.NotFound,
            Messages = messages ?? new List<string> {"The resource was not found."}
        };

        public static ErrorDetails Generate400Error(List<string> messages) => new ErrorDetails()
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            Messages = messages ?? new List<string> {"The information provider is not valid."}
        };

        public static ErrorDetails GenerateEntityAlreadyExistsError(List<string> messages) => new ErrorDetails()
        {
            StatusCode = (int)HttpStatusCode.UnprocessableEntity,
            Messages = messages ?? new List<string>{"That item with the same name already exists."}
        };
    }
}
