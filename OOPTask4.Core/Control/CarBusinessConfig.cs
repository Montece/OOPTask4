namespace OOPTask4.Core.Control;

public sealed class CarBusinessConfig(
    int workersCount,
    int suppliersCount,
    int dealersCount,
    int warehouseCarcaseCapacity,
    int warehouseAccessoryCapacity,
    int warehouseEngineCapacity,
    int warehouseCarCapacity)
{
    public int WorkersCount { get; } = workersCount;
    public int SuppliersCount { get; } = suppliersCount;
    public int DealersCount { get; } = dealersCount;
    public int WarehouseCarcaseCapacity { get; } = warehouseCarcaseCapacity;
    public int WarehouseAccessoryCapacity { get; } = warehouseAccessoryCapacity;
    public int WarehouseEngineCapacity { get; } = warehouseEngineCapacity;
    public int WarehouseCarCapacity { get; } = warehouseCarCapacity;
}