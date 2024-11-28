using OOPTask4.Core.Products;
using OOPTask4.Core.Tickable;
using OOPTask4.Core.Warehouse;

namespace OOPTask4.Core.Supplier;

public sealed class Supplier<T> : ITickable where T : Component
{
    private readonly Type _supplyComponentType = typeof(T);
    private Warehouse<T>? _warehouse;

    public void BindToWarehouse(Warehouse<T> warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);
        
        _warehouse = warehouse;
    }

    public void Tick()
    {
        Supply();
    }

    private void Supply()
    {
        if (_warehouse is null)
        {
            throw new NotSpecifiedWarehouseException();
        }

        if (Activator.CreateInstance(_supplyComponentType) is not T supplyComponent)
        {
            throw new ComponentMismatchException();
        }

        var addResult = false;

        do
        {
            addResult = _warehouse.AddProduct(supplyComponent);
            
            if (!addResult)
            {
                _warehouse.IsNotFullAndNotEmpty.WaitForTurnOn();
            }
        }
        while (!addResult);
    }

    public override string ToString()
    {
        return "Supplier";
    }
}