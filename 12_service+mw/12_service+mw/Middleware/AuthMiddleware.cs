using _12_service_mw.Services;

namespace _12_service_mw.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<AuthMiddleware> logger;

    public AuthMiddleware(
        RequestDelegate next,
        ILogger<AuthMiddleware> logger )
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IRequestCounter counter
    )
    {
        logger.LogInformation($"Auth check for request #{counter.Increment}");

        if (! context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        await next(context);
    }
    
}
