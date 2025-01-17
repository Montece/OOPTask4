using OOPTask4.Core.Control;
using OOPTask4.Core.Products;
using OOPTask4.Core.Warehouse;
using OOPTask4.Threading.Tickable;

namespace OOPTask4.Core.Deal;

public sealed class Dealer : Entity, ITickable
{
    public long CarsSoldCount => _carsSoldCount;
    private long _carsSoldCount;

    private readonly Warehouse<Car> _sourceWarehouseOfProduct;
    private readonly ControllerManager _controllerManager;

    public Dealer(ControllerManager controllerManager, Warehouse<Car> warehouse)
    {
        ArgumentNullException.ThrowIfNull(controllerManager);
        ArgumentNullException.ThrowIfNull(warehouse);

        _controllerManager = controllerManager;
        _sourceWarehouseOfProduct = warehouse;
    }

    public override void Tick()
    {
        Deal();
    }

    private void Deal()
    {
        _ = GetProductFromWarehouse(_sourceWarehouseOfProduct, _controllerManager);

        Interlocked.Increment(ref _carsSoldCount);
    }

    public override object Clone()
    {
        var dealer = new Dealer(_controllerManager, _sourceWarehouseOfProduct);
        
        return dealer;
    }
}