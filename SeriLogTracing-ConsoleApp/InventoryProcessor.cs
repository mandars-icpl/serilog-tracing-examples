using System.Diagnostics;
using Serilog;

public class InventoryProcessor
{
    private readonly ActivitySource _activitySource;

    public InventoryProcessor(ActivitySource activitySource)
    {
        _activitySource = activitySource;
    }

    public void UpdateInventory(int orderId)
    {
        using var inventoryActivity = _activitySource.StartActivity($"UpdateInventory-{orderId}", ActivityKind.Client);
        inventoryActivity?.SetTag("order.id", orderId);
        inventoryActivity?.SetTag("inventory.status", "Updated");

        Log.Information("Updating inventory for order {OrderId}", orderId);

        Task.Delay(500).Wait();  // Simulate inventory update delay

        Log.Information("Inventory for order {OrderId} updated", orderId);
        inventoryActivity?.Stop();
    }
}
