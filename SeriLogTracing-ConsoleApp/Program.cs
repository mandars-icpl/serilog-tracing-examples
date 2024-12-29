using Serilog;
using System.Diagnostics;

Log.Logger = new LoggerConfiguration()
           .Enrich.WithProperty("Application", "ECommerceOrderProcessor")
           .WriteTo.Console()
           .WriteTo.Seq("http://localhost:5341")
           .CreateLogger();

Log.Information("E-commerce Order Processor Starting Up");

var activitySource = new ActivitySource("ECommerceOrderProcessor");

for (int i = 1; i <= 3; i++)
{
    ProcessOrder(activitySource, i);
}

Log.Information("E-commerce Order Processor Shutting Down");
Log.CloseAndFlush();



static void ProcessOrder(ActivitySource activitySource, int orderId)
{
    using var orderActivity = activitySource.StartActivity($"ProcessOrder-{orderId}", ActivityKind.Server);
    orderActivity?.SetTag("order.id", orderId);
    orderActivity?.SetTag("customer.id", $"CUST-{1000 + orderId}");
    orderActivity?.SetTag("order.status", "Processing");

    Log.Information("Processing order {OrderId} for customer {CustomerId}", orderId, $"CUST-{1000 + orderId}");

    ProcessPayment(activitySource, orderId);
    UpdateInventory(activitySource, orderId);

    Log.Information("Order {OrderId} processed successfully", orderId);
}

static void ProcessPayment(ActivitySource activitySource, int orderId)
{
    using var paymentActivity = activitySource.StartActivity($"ProcessPayment-{orderId}", ActivityKind.Client);
    paymentActivity?.SetTag("order.id", orderId);
    paymentActivity?.SetTag("payment.status", "Success");

    Log.Information("Processing payment for order {OrderId}", orderId);

    Task.Delay(500).Wait();

    Log.Information("Payment for order {OrderId} completed", orderId);
}

static void UpdateInventory(ActivitySource activitySource, int orderId)
{
    using var inventoryActivity = activitySource.StartActivity($"UpdateInventory-{orderId}", ActivityKind.Client);
    inventoryActivity?.SetTag("order.id", orderId);
    inventoryActivity?.SetTag("inventory.status", "Updated");

    Log.Information("Updating inventory for order {OrderId}", orderId);

    Task.Delay(500).Wait();

    Log.Information("Inventory for order {OrderId} updated", orderId);
}