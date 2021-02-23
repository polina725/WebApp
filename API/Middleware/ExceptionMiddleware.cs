using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using API.Errors;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var res = _env.IsDevelopment() 
                            ? new ApiException(context.Response.StatusCode, e.Message, e.StackTrace?.ToString())
                            : new ApiException(context.Response.StatusCode, "Internal server error");
                
                var opt = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(res, opt);

                await context.Response.WriteAsync(json);
            }
        }
    }
}