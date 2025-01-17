using OOPTask4.Core.Control;
using OOPTask4.Core.Deal;
using OOPTask4.Core.Products;
using Xunit;
using OOPTask4.Core.Warehouse;
using OOPTask4.Core.Supply;
using OOPTask4.Core.Work;
using OOPTask4.Threading;

namespace OOPTask4.Core.Tests;

public sealed class CombinationsTests
{
    [Fact]
    public void Combination_Supplier_Worker_Dealer_Warehouse_CarsCount()
    {
        using var carBusiness = new CarBusiness(new(1, 1, 1, 1, 1, 1, 1), 1);
        var controller = new ControllerManager(carBusiness);
        var pool = new TickableThreadPool(10);

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

        var supplier1Ticks = 9;
        pool.DoTick(supplier1, supplier1Ticks);
        Delay(supplier1Ticks);

        var supplier2Ticks = 7;
        pool.DoTick(supplier2, supplier2Ticks);
        Delay(supplier2Ticks);

        var supplier3Ticks = 8;
        pool.DoTick(supplier3, supplier3Ticks);
        Delay(supplier3Ticks);

        var worker1Ticks = 2;
        pool.DoTick(worker1, worker1Ticks);
        Delay(worker1Ticks);

        var worker2Ticks = 6;
        pool.DoTick(worker2, worker2Ticks);
        Delay(worker2Ticks);

        var dealer1Ticks = 5;
        pool.DoTick(dealer1, dealer1Ticks);
        Delay(dealer1Ticks);

        Delay(10);
        
        Assert.Equal(1, warehouseCarcase.ProductsCount);
        Assert.Equal(0, warehouseEngine.ProductsCount);
        Assert.Equal(1, warehouseAccessory.ProductsCount);
        Assert.Equal(2, warehouseCar.ProductsCount);
    }

    private void Delay(int ticksCount)
    {
        Thread.Sleep(10 * ticksCount);
    }
}