using BankingSystem.Domain.Exceptions;
using BankingSystem.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace BankingSystem.WebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private const string JsonContentType = "application/json";
        private readonly JsonSerializerSettings SerializerSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        private readonly RequestDelegate _request;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _request = next;
            _logger = logger;
        }

        public Task Invoke(HttpContext context) => this.InvokeAsync(context);

        async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (BankingException e)
            {
                context.Response.ContentType = JsonContentType;
                context.Response.StatusCode = (int)e.ErrorCode;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponseDTO(e.Message), SerializerSettings));
            }
            catch (Exception)
            {
                var response = new ErrorResponseDTO("Unhandled Server Error");

                // set http status code and content type
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = JsonContentType;

                // writes / returns error model to the response
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, SerializerSettings));
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
