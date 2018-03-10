using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RealWorld.Infrastructure.Errors
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async  Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is RestException re)
            {
                context.Response.StatusCode = (int)re.Code;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                if (!string.IsNullOrWhiteSpace(exception.Message))
                {
                    context.Response.ContentType = "application/json";
                    var result = JsonConvert.SerializeObject(
                        new
                        {
                            errors = exception.Message
                        });
                    await context.Response.WriteAsync(result);
                }
            }
        }
    }
}
