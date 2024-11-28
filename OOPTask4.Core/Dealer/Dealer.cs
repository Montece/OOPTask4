using OOPTask4.Core.Products;
using OOPTask4.Core.Supplier;
using OOPTask4.Core.Tickable;
using OOPTask4.Core.Warehouse;

namespace OOPTask4.Core.Dealer;

public sealed class Dealer<T> : ITickable where T : Product
{
    private Warehouse<T>? _sourceWarehouseOfProduct;

    public void BindToSourceWarehouseOfProduct(Warehouse<T> warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);

        _sourceWarehouseOfProduct = warehouse;
    }

    public void Tick()
    {
        Deal();
    }

    private void Deal()
    {
        if (_sourceWarehouseOfProduct is null)
        {
            throw new NotSpecifiedWarehouseException();
        }

        Product? bufferedProduct = null;

        while (bufferedProduct is null)
        {
            bufferedProduct = _sourceWarehouseOfProduct.GetProduct() as T;

            if (bufferedProduct is null)
            {
                _sourceWarehouseOfProduct.IsNotFullAndNotEmpty.WaitForTurnOn();
            }
        }
    }
}