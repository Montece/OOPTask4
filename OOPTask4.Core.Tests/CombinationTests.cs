using OOPTask4.Core.Products;
using Xunit;
using OOPTask4.Core.Warehouse;
using OOPTask4.Core.Supplier;
using OOPTask4.Core.Tickable;
using OOPTask4.Threading;

namespace OOPTask4.Core.Tests;

public class CombinationTests
{
    private readonly RunnablesPool _pool = new();

    [Fact]
    public void A()
    {
        var warehouseCarcase = new Warehouse<Carcase>(100000);
        var warehouseEngine = new Warehouse<Engine>(100000);
        var warehouseAccessory = new Warehouse<Accessory>(100000);
        var warehouseCar = new Warehouse<Car>(100000);

        var supplier1 = new Supplier<Carcase>();
        supplier1.BindToWarehouse(warehouseCarcase);
        var supplier2 = new Supplier<Engine>();
        supplier2.BindToWarehouse(warehouseEngine);
        var supplier3 = new Supplier<Accessory>();
        supplier3.BindToWarehouse(warehouseAccessory);

        var worker = new Worker.Worker<Carcase, Engine, Accessory, Car>();

        worker.BindToSourceWarehouseOfComponent1(warehouseCarcase);
        worker.BindToSourceWarehouseOfComponent2(warehouseEngine);
        worker.BindToSourceWarehouseOfComponent3(warehouseAccessory);
        worker.BindToTargetWarehouse(warehouseCar);

        _pool.AddAndStart(new TickableRunnableTarget(new(supplier1), TimeSpan.FromMilliseconds(1)));
        _pool.AddAndStart(new TickableRunnableTarget(new(supplier2), TimeSpan.FromMilliseconds(1)));
        _pool.AddAndStart(new TickableRunnableTarget(new(supplier3), TimeSpan.FromMilliseconds(1)));
        _pool.AddAndStart(new TickableRunnableTarget(new(worker), TimeSpan.FromMilliseconds(1)));

        Thread.Sleep(1000);
    }
}