using OOPTask4.Core.Products;
using OOPTask4.Core.Warehouse;
using OOPTask4.Threading.Tickable;

namespace OOPTask4.Core.Supply;

public sealed class Supplier<T> : Entity, ITickable where T : Component
{
    private readonly Type _supplyComponentType = typeof(T);
    private readonly Warehouse<T> _warehouse;

    public Supplier(Warehouse<T> targetWarehouse)
    {
        ArgumentNullException.ThrowIfNull(targetWarehouse);

        _warehouse = targetWarehouse;
    }

    public override void Tick()
    {
        Supply();
    }

    private void Supply()
    {
        T? supplyComponent;

        do
        { 
            supplyComponent = Activator.CreateInstance(_supplyComponentType) as T;
        }
        while (supplyComponent == null);

        bool addResult;

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

    public override object Clone()
    {
        var supplier = new Supplier<T>(_warehouse);

        return supplier;
    }
}