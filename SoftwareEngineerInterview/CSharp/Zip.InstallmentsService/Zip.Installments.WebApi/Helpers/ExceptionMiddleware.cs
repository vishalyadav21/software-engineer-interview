using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Resources;
using System.Threading.Tasks;

namespace Zip.Installments.WebApi.Helpers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly ResourceManager resourceManager;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
            this.resourceManager = new
            ResourceManager("Zip.Installments.WebApi.Resources.Resource", typeof(ExceptionMiddleware).Assembly);
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await this.next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            ErrorResponse response = new ErrorResponse();

            if (exception is CustomExceptionFilter)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                response.ErrorMessage = exception.Message;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.ErrorMessage = exception.Message;
                response.StackTrace = exception.StackTrace;
            }

            //  this.logger.LogCritical(exception, exception.Message, GetProperties(context));
            return context.Response.WriteAsync(response.ErrorMessage.ToString() + " \n \n \n" + response.StackTrace.ToString());
        }
    }
}
