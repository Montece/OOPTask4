using OOPTask4.Core.Products;
using OOPTask4.Core.Warehouse;
using OOPTask4.Threading.Tickable;

namespace OOPTask4.Core.Work;

public sealed class Worker : Entity, ITickable
{
    private readonly Warehouse<Carcase> _sourceWarehouseOfCarcases;
    private readonly Warehouse<Engine> _sourceWarehouseOfEngines;
    private readonly Warehouse<Accessory> _sourceWarehouseOfAccessories;
    private readonly Warehouse<Car> _targetWarehouseOfCars;

    public Worker(Warehouse<Carcase> carcasesWarehouse, Warehouse<Engine> enginesWarehouse, Warehouse<Accessory> accessoriesWarehouse, Warehouse<Car> carsWarehouse)
    {
        ArgumentNullException.ThrowIfNull(carcasesWarehouse);
        ArgumentNullException.ThrowIfNull(enginesWarehouse);
        ArgumentNullException.ThrowIfNull(accessoriesWarehouse);
        ArgumentNullException.ThrowIfNull(carsWarehouse);

        _sourceWarehouseOfCarcases = carcasesWarehouse;
        _sourceWarehouseOfEngines = enginesWarehouse;
        _sourceWarehouseOfAccessories = accessoriesWarehouse;
        _targetWarehouseOfCars = carsWarehouse;
    }

    public override void Tick()
    {
        Work();
    }

    private void Work()
    {
        _ = GetProductFromWarehouse(_sourceWarehouseOfCarcases);
        _ = GetProductFromWarehouse(_sourceWarehouseOfEngines);
        _ = GetProductFromWarehouse(_sourceWarehouseOfAccessories);

        Car? bufferedTarget1 = null;

        do
        {
            bufferedTarget1 ??= Activator.CreateInstance(typeof(Car)) as Car;
        }
        while (bufferedTarget1 is null);

        bool addResult;

        do
        {
            addResult = _targetWarehouseOfCars.AddProduct(bufferedTarget1);
            
            if (!addResult)
            {
                _targetWarehouseOfCars.IsNotFullAndNotEmpty.WaitForTurnOn();
            }
        }
        while (!addResult);
    }

    public override object Clone()
    {
        var worker = new Worker(_sourceWarehouseOfCarcases, _sourceWarehouseOfEngines, _sourceWarehouseOfAccessories, _targetWarehouseOfCars);
        
        return worker;
    }
}