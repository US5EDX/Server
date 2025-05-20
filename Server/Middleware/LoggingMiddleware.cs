namespace Server.Middleware;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        logger.LogInformation("Запит: {Method} {Path}; Дата та час (UTC): {TimeStamp}",
            context.Request.Method, context.Request.Path, DateTime.UtcNow);
        await next(context);
    }
}
