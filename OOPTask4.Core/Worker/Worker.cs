using OOPTask4.Core.Products;
using OOPTask4.Core.Supplier;
using OOPTask4.Core.Tickable;
using OOPTask4.Core.Warehouse;

namespace OOPTask4.Core.Worker;

public sealed class Worker<TI1, TI2, TI3, TO> : ITickable
    where TI1 : Component
    where TI2 : Component
    where TI3 : Component
    where TO : Product
{
    private readonly Type _supplyType1 = typeof(TI1);
    private readonly Type _supplyType2 = typeof(TI2);
    private readonly Type _supplyType3 = typeof(TI3);
    private readonly Type _targetType = typeof(TO);

    private Warehouse<TI1> _sourceWarehouseOfComponent1;
    private Warehouse<TI2> _sourceWarehouseOfComponent2;
    private Warehouse<TI3> _sourceWarehouseOfComponent3;
    private Warehouse<TO> _targetWarehouse;

    private TI1? _bufferedComponent1;
    private TI2? _bufferedComponent2;
    private TI3? _bufferedComponent3;
    private TO? _bufferedTarget1;

    public void BindToSourceWarehouseOfComponent1(Warehouse<TI1> warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);

        _sourceWarehouseOfComponent1 = warehouse;
    }

    public void BindToSourceWarehouseOfComponent2(Warehouse<TI2> warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);

        _sourceWarehouseOfComponent2 = warehouse;
    }

    public void BindToSourceWarehouseOfComponent3(Warehouse<TI3> warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);

        _sourceWarehouseOfComponent3 = warehouse;
    }

    public void BindToTargetWarehouse(Warehouse<TO> warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);
        
        _targetWarehouse = warehouse;
    }

    public void Tick()
    {
        Work();
    }

    private void Work()
    {
        if (_sourceWarehouseOfComponent1 is null || _sourceWarehouseOfComponent2 is null || _sourceWarehouseOfComponent3 is null || _targetWarehouse is null)
        {
            throw new NotSpecifiedWarehouseException();
        }

        while (_bufferedComponent1 is null)
        {
            _bufferedComponent1 = _sourceWarehouseOfComponent1.GetProduct() as TI1;

            if (_bufferedComponent1 is null)
            {
                _sourceWarehouseOfComponent1.IsNotFullAndNotEmpty.WaitForTurnOn();
            }
        }

        while (_bufferedComponent2 is null)
        {
            _bufferedComponent2 = _sourceWarehouseOfComponent2.GetProduct() as TI2;

            if (_bufferedComponent2 is null)
            {
                _sourceWarehouseOfComponent2.IsNotFullAndNotEmpty.WaitForTurnOn();
            }
        }

        while (_bufferedComponent3 is null)
        {
            _bufferedComponent3 = _sourceWarehouseOfComponent3.GetProduct() as TI3;

            if (_bufferedComponent3 is null)
            {
                _sourceWarehouseOfComponent3.IsNotFullAndNotEmpty.WaitForTurnOn();
            }
        }

        _bufferedTarget1 ??= Activator.CreateInstance(_targetType) as TO;

        if (_bufferedTarget1 is null)
        {
            throw new ProductMismatchException();
        }

        var addResult = false;

        do
        {
            addResult = _targetWarehouse.AddProduct(_bufferedTarget1);
            
            if (!addResult)
            {
                _targetWarehouse.IsNotFullAndNotEmpty.WaitForTurnOn();
            }
        }
        while (!addResult);
    }
}