using _12_service_mw.Services;

namespace _12_service_mw.Middleware;

public class ServiceInjectionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ITimeService timeService;

    public ServiceInjectionMiddleware(
        RequestDelegate next,
        ITimeService timeService)
    {
        this.next = next;
        this.timeService = timeService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("X-Server-Time", timeService.GetTime());

        await next(context);
    }
    
}
