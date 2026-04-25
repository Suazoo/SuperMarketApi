namespace SuperMarketAPI.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Antes del request
        _logger.LogInformation(
            "➡️ {Method} {Path} | {Time}",
            context.Request.Method,
            context.Request.Path,
            DateTime.Now.ToString("HH:mm:ss"));

        await _next(context);

        // Después del request
        _logger.LogInformation(
            "⬅️ {Method} {Path} | Status: {StatusCode}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode);
    }
}