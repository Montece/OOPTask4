using JetBrains.Annotations;
using OOPTask4.Core.Control;
using OOPTask4.Core.Products;
using OOPTask4.Core.Warehouse;
using OOPTask4.Threading.Tickable;

namespace OOPTask4.Core;

public abstract class Entity : UniqueObject, ITickable
{
    [MustUseReturnValue]
    protected static T GetProductFromWarehouse<T>(Warehouse<T> warehouse, ControllerManager? controllerManager = null) where T : Product
    {
        T? bufferedProduct = null;

        while (bufferedProduct is null)
        {
            bufferedProduct = warehouse.GetProduct() as T;

            if (bufferedProduct is null)
            {
                controllerManager?.DoRequestForSupply();
                warehouse.IsNotFullAndNotEmpty.WaitForTurnOn();
            }
        }

        return bufferedProduct;
    }

    public abstract object Clone();

    public abstract void Tick();
}