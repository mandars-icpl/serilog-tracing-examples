using System.Diagnostics;
using Serilog;

public class OrderProcessor
{
    private readonly ActivitySource _activitySource;

    public OrderProcessor(ActivitySource activitySource)
    {
        _activitySource = activitySource;
    }

    public void ProcessOrder(int orderId)
    {
        using var orderActivity = _activitySource.StartActivity($"ProcessOrder-{orderId}", ActivityKind.Server);
        orderActivity?.SetTag("order.id", orderId);
        orderActivity?.SetTag("customer.id", $"CUST-{1000 + orderId}");
        orderActivity?.SetTag("order.status", "Processing");

        Log.Information("Processing order {OrderId} for customer {CustomerId}", orderId, $"CUST-{1000 + orderId}");

        // Process payment and update inventory
        var paymentProcessor = new PaymentProcessor(_activitySource);
        paymentProcessor.ProcessPayment(orderId);


        var inventoryProcessor = new InventoryProcessor(_activitySource);
        inventoryProcessor.UpdateInventory(orderId);

        Log.Information("Order {OrderId} processed successfully", orderId);
        orderActivity?.Stop();
    }
}
