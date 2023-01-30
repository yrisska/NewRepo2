using RssManager.Middlewares.Models;
using System.Net;

namespace RssManager.Middlewares
{
    //Sort of global exception handler, it looks a little hardcoded, but works fine
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                httpContext.Response.StatusCode = e switch
                {
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    BadHttpRequestException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                if (httpContext.Response.StatusCode >= 500)
                    _logger.LogCritical(e.ToString());
                else
                    _logger.LogError(e.ToString());

                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = httpContext.Response.StatusCode >= 500 ? "Something went wrong" : e.Message,
                }.ToString());
            }
        }
    }
}
