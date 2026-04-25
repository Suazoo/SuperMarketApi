using System.Diagnostics;

namespace SuperMarketAPI.Middleware;

public class ResponseTimeMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        context.Response.OnStarting(() =>
        {
            sw.Stop();
            context.Response.Headers["X-Response-Time"] = $"{sw.ElapsedMilliseconds}ms";
            return Task.CompletedTask;
        });

        await _next(context);
    }
}