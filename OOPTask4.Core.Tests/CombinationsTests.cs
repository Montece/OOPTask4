using OOPTask4.Core.Control;
using OOPTask4.Core.Deal;
using OOPTask4.Core.Products;
using Xunit;
using OOPTask4.Core.Warehouse;
using OOPTask4.Core.Supply;
using OOPTask4.Core.Work;
using OOPTask4.Threading;
using OOPTask4.Threading.Tickable;

namespace OOPTask4.Core.Tests;

public sealed class CombinationsTests
{
    private readonly TimeSpan _tickPeriod = TimeSpan.FromMilliseconds(1);

    [Fact]
    public void Combination_Supplier_Worker_Dealer_Warehouse_CarsCount()
    {
        using var carBusiness = new CarBusiness(new(1, 1, 1, 1, 1, 1, 1), 1);
        var controller = new ControllerManager(carBusiness);
        var pool = new RunnablesPool();

        using var warehouseCarcase = new Warehouse<Carcase>(100000);
        using var warehouseEngine = new Warehouse<Engine>(100000);
        using var warehouseAccessory = new Warehouse<Accessory>(100000);
        using var warehouseCar = new Warehouse<Car>(100000);

        var supplier1 = new Supplier<Carcase>(warehouseCarcase);
        var supplier2 = new Supplier<Engine>(warehouseEngine);
        var supplier3 = new Supplier<Accessory>(warehouseAccessory);

        var worker1 = new Worker(warehouseCarcase, warehouseEngine, warehouseAccessory, warehouseCar);
        var worker2 = new Worker(warehouseCarcase, warehouseEngine, warehouseAccessory, warehouseCar);

        var dealer1 = new Dealer(controller, warehouseCar);

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