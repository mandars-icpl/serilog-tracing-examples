using System.Diagnostics;
using Serilog;

public class PaymentProcessor
{
    private readonly ActivitySource _activitySource;

    public PaymentProcessor(ActivitySource activitySource)
    {
        _activitySource = activitySource;
    }

    public void ProcessPayment(int orderId)
    {
        using var paymentActivity = _activitySource.StartActivity($"ProcessPayment-{orderId}", ActivityKind.Client);
        paymentActivity?.SetTag("order.id", orderId);
        paymentActivity?.SetTag("payment.status", "Success");

        Log.Information("Processing payment for order {OrderId}", orderId);

        Task.Delay(500).Wait();  // Simulate payment processing delay

        Log.Information("Payment for order {OrderId} completed", orderId);
        paymentActivity?.Stop();
    }
}
