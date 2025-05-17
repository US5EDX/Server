namespace Server.Middleware;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        logger.LogInformation($"Запит: {context.Request.Method} {context.Request.Path}; Дата та час (UTC): {DateTime.UtcNow}");
        await next(context);
    }
}
