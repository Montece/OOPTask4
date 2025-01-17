using OOPTask4.Core.Deal;
using OOPTask4.Core.Products;
using OOPTask4.Core.Supply;
using OOPTask4.Core.Warehouse;
using OOPTask4.Core.Work;
using OOPTask4.Threading;
using OOPTask4.Threading.Tickable;

namespace OOPTask4.Core.Control;

public sealed class CarBusiness : IDisposable
{
    public Warehouse<Carcase> WarehouseCarcase { get; }
    public Warehouse<Accessory> WarehouseAccessory { get; }
    public Warehouse<Engine> WarehouseEngine { get; }
    public Warehouse<Car> WarehouseCar { get; }

    public TickableGroup<Worker> Workers { get; }
    public TickableGroup<Supplier<Carcase>> SuppliersCarcase { get; }
    public TickableGroup<Supplier<Accessory>> SuppliersAccessories { get; }
    public TickableGroup<Supplier<Engine>> SuppliersEngine { get; }
    public TickableGroup<Dealer> Dealers { get; }

    public TimeSpan CarDealDelay { get; } = TimeSpan.FromMilliseconds(250);
    public bool BusinessIsRunning { get; private set; } = true;

    private bool _disposed;

    public CarBusiness(CarBusinessConfig config, int threadsCount)
    {
        var runnersPool = new TickableThreadPool(threadsCount);

        var controllerManager = new ControllerManager(this);

        WarehouseCarcase = new(config.WarehouseCarcaseCapacity);
        WarehouseAccessory = new(config.WarehouseAccessoryCapacity);
        WarehouseEngine = new(config.WarehouseEngineCapacity);
        WarehouseCar = new(config.WarehouseCarCapacity);

        var worker = new Worker(WarehouseCarcase, WarehouseEngine, WarehouseAccessory, WarehouseCar);
        var supplierCarcase = new Supplier<Carcase>(WarehouseCarcase);
        var supplierAccessory = new Supplier<Accessory>(WarehouseAccessory);
        var supplierEngine = new Supplier<Engine>(WarehouseEngine);
        var dealer = new Dealer(controllerManager, WarehouseCar);

        Workers = new(runnersPool, worker, config.WorkersCount);
        SuppliersCarcase = new(runnersPool, supplierCarcase, config.SuppliersCount);
        SuppliersAccessories = new(runnersPool, supplierAccessory, config.SuppliersCount);
        SuppliersEngine = new(runnersPool, supplierEngine, config.SuppliersCount);
        Dealers = new(runnersPool, dealer, config.DealersCount);

        Task.Run(ProcessBusiness);
    }

    private void ProcessBusiness()
    {
        if (_disposed)
        {
            throw new InvalidOperationException("Object was disposed!");
        }

        while (BusinessIsRunning)
        {
            if (_disposed)
            {
                throw new InvalidOperationException("Object was disposed!");
            }

            Dealers.DoWork();
            Task.Delay(CarDealDelay).Wait();
        }
    }

    public void StopBusiness()
    {
        if (_disposed)
        {
            throw new InvalidOperationException("Object was disposed!");
        }

        BusinessIsRunning = false;
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            WarehouseCarcase.Dispose();
            WarehouseAccessory.Dispose();
            WarehouseEngine.Dispose();
            WarehouseCar.Dispose();
        }

        _disposed = true;
    }

    ~CarBusiness()
    {
        Dispose(false);
    }
}