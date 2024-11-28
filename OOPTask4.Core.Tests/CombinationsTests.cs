using OOPTask4.Core.Dealer;
using OOPTask4.Core.Products;
using Xunit;
using OOPTask4.Core.Warehouse;
using OOPTask4.Core.Supplier;
using OOPTask4.Core.Tickable;
using OOPTask4.Threading;
using OOPTask4.Core.Worker;

namespace OOPTask4.Core.Tests;

public class CombinationsTests
{
    private readonly TimeSpan _tickPeriod = TimeSpan.FromMilliseconds(1);

    [Fact]
    public void Combination_Supplier_Worker_Dealer_Warehouse_CarsCount()
    {
        var pool = new RunnablesPool();

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

        var worker1 = new Worker<Carcase, Engine, Accessory, Car>();
        var worker2 = new Worker<Carcase, Engine, Accessory, Car>();

        var dealer1 = new Dealer<Car>();

        worker1.BindToSourceWarehouseOfComponent1(warehouseCarcase);
        worker1.BindToSourceWarehouseOfComponent2(warehouseEngine);
        worker1.BindToSourceWarehouseOfComponent3(warehouseAccessory);
        worker1.BindToTargetWarehouse(warehouseCar);

        worker2.BindToSourceWarehouseOfComponent1(warehouseCarcase);
        worker2.BindToSourceWarehouseOfComponent2(warehouseEngine);
        worker2.BindToSourceWarehouseOfComponent3(warehouseAccessory);
        worker2.BindToTargetWarehouse(warehouseCar);

        dealer1.BindToSourceWarehouseOfProduct(warehouseCar);

        var tickable1 = pool.AddAndStart(new TickableRunnableTarget(new(supplier1), _tickPeriod, 9));
        var tickable2 = pool.AddAndStart(new TickableRunnableTarget(new(supplier2), _tickPeriod, 7));
        var tickable3 = pool.AddAndStart(new TickableRunnableTarget(new(supplier3), _tickPeriod, 8));
        var tickable4 = pool.AddAndStart(new TickableRunnableTarget(new(worker1), _tickPeriod, 2));
        var tickable5 = pool.AddAndStart(new TickableRunnableTarget(new(worker2), _tickPeriod, 6));
        var tickable6 = pool.AddAndStart(new TickableRunnableTarget(new(dealer1), _tickPeriod, 5));

        Thread.Sleep(1000);

        pool.RemoveAndStop(tickable1);
        pool.RemoveAndStop(tickable2);
        pool.RemoveAndStop(tickable3);
        pool.RemoveAndStop(tickable4);
        pool.RemoveAndStop(tickable5);
        pool.RemoveAndStop(tickable6);
        
        Assert.Equal(1, warehouseCarcase.ProductsCount);
        Assert.Equal(0, warehouseEngine.ProductsCount);
        Assert.Equal(1, warehouseAccessory.ProductsCount);
        Assert.Equal(2, warehouseCar.ProductsCount);
    }
}