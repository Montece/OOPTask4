namespace OOPTask4.Core.Control;

public sealed class CarBusinessConfig
{
    public int WorkersCount { get; }
    public int SuppliersCount { get; }
    public int DealersCount { get; }
    public int WarehouseCarcaseCapacity { get; }
    public int WarehouseAccessoryCapacity { get; }
    public int WarehouseEngineCapacity { get; }
    public int WarehouseCarCapacity { get; }

    public CarBusinessConfig(int workersCount, int suppliersCount, int dealersCount, int warehouseCarcaseCapacity, int warehouseAccessoryCapacity, int warehouseEngineCapacity, int warehouseCarCapacity)
    {
        Utility.CheckIfGreaterThanZero(workersCount, nameof(workersCount));
        Utility.CheckIfGreaterThanZero(suppliersCount, nameof(suppliersCount));
        Utility.CheckIfGreaterThanZero(dealersCount, nameof(dealersCount));
        Utility.CheckIfGreaterThanZero(warehouseCarcaseCapacity, nameof(warehouseCarcaseCapacity));
        Utility.CheckIfGreaterThanZero(warehouseAccessoryCapacity, nameof(warehouseAccessoryCapacity));
        Utility.CheckIfGreaterThanZero(warehouseEngineCapacity, nameof(warehouseEngineCapacity));
        Utility.CheckIfGreaterThanZero(warehouseCarCapacity, nameof(warehouseCarCapacity));

        WorkersCount = workersCount;
        SuppliersCount = suppliersCount;
        DealersCount = dealersCount;
        WarehouseCarcaseCapacity = warehouseCarcaseCapacity;
        WarehouseAccessoryCapacity = warehouseAccessoryCapacity;
        WarehouseEngineCapacity = warehouseEngineCapacity;
        WarehouseCarCapacity = warehouseCarCapacity;
    }
}