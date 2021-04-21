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
using BadHttpRequestException = Microsoft.AspNetCore.Server.IIS.BadHttpRequestException;

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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            switch (exception)
            {
                case EntityAlreadyExistsException entityAlreadyExistsException:
                    return context.Response.WriteAsync(ErrorDetails.GenerateEntityAlreadyExistsError().ToString());
                case EntityNotFoundException entityNotFoundException:
                    return context.Response.WriteAsync(ErrorDetails.Generate404Error().ToString());
                default:
                    return context.Response.WriteAsync(ErrorDetails.Generate500Error().ToString());
            }
        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString() => JsonSerializer.Serialize(this);

        public static ErrorDetails Generate500Error() => new ErrorDetails()
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = "An unexpected error has occurred, please contact support."
        };

        public static ErrorDetails Generate404Error() => new ErrorDetails()
        {
            StatusCode = (int)HttpStatusCode.NotFound,
            Message = "Sorry that resource is not found, please contact support."
        };

        public static ErrorDetails GenerateEntityAlreadyExistsError() => new ErrorDetails()
        {
            StatusCode = (int)HttpStatusCode.UnprocessableEntity,
            Message = "Sorry that already exists, please try a different one."
        };
    }
}
