using _12_service_mw.Services;

namespace _12_service_mw.Middleware;

public class ScopedServiceMiddleware
{
    private readonly RequestDelegate next;
    private readonly IRequestCounter counter;

    public ScopedServiceMiddleware(RequestDelegate next, IRequestCounter counter)
    {
        this.next = next;
        this.counter = counter;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        var requestNumber = counter.Increment();
        context.Items["RequestNumber"] = requestNumber;
        await next(context);
    }
}
