var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IPaymentProcessor, StripeProceesor>();
builder.Services.AddTransient<IPaymentProcessor, PayPalProceesor>();
builder.Services.AddTransient<IPaymentProcessor, BankProceesor>();

builder.Services.AddScoped<OrderService>();

var app = builder.Build();

app.MapGet("/process", (OrderService service) => service.ProcessOrder(150.00m));

app.Run();


interface IPaymentProcessor
{
    string Process(decimal amount);
}
class StripeProceesor : IPaymentProcessor
{
    public string Process(decimal amount)
        => $"Processed {amount} via Stripe";
}
class PayPalProceesor : IPaymentProcessor
{
    public string Process(decimal amount)
        => $"Processed {amount} via PayPal";
}
class BankProceesor : IPaymentProcessor
{
    public string Process(decimal amount)
        => $"Processed {amount} via Bank";
}

class OrderService
{
    private readonly IEnumerable<IPaymentProcessor> processors;
    public OrderService(IEnumerable<IPaymentProcessor> processors)
    {
        this.processors = processors;
    }
    public List<string> ProcessOrder(decimal amount)
    {
        return processors.Select(p => p.Process(amount)).ToList();
    }
}
