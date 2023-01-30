using RssManager.Middlewares;

namespace RssManager.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder) =>
            builder.UseMiddleware<ExceptionHandlingMiddleware>();

        public static IApplicationBuilder UseDbTransaction(this IApplicationBuilder builder) =>
            builder.UseMiddleware<DbTransactionMiddleware>();
    }
}
