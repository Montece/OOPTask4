using OOPTask4.Core.Products;
using OOPTask4.Threading;

namespace OOPTask4.Core.Warehouse;

public class Warehouse<T> where T : Product
{
    public Signal IsFull { get; private set; } = new(false);
    public Signal IsNotFullAndNotEmpty { get; private set; } = new(false);
    public Signal IsEmpty { get; private set; } = new(true);
    
    public int ProductsCount
    {
        get
        {
            lock (_lock)
            {
                return _products.Count;
            }
        }
    }

    private readonly Stack<T> _products = [];

    private readonly object _lock = new();
    private readonly int _maxCapacity;

    public Warehouse(int maxCapacity)
    {
        if (maxCapacity <= 0)
        {
            throw new ArgumentException("Must be greater than zero", nameof(maxCapacity));
        }

        _maxCapacity = maxCapacity;
    }

    public bool AddProduct(T product)
    {
        ArgumentNullException.ThrowIfNull(product);

        lock (_lock)
        {
            if (_products.Count >= _maxCapacity)
            {
                return false;
            }

            _products.Push(product);

            IsEmpty.TurnOff();

            if (_products.Count >= _maxCapacity)
            {
                IsFull.TurnOn();
            }
            else
            {
                IsFull.TurnOff();
            }

            if (_products.Count > 0 && _products.Count < _maxCapacity)
            {
                IsNotFullAndNotEmpty.TurnOn();
            }
            else
            {
                IsNotFullAndNotEmpty.TurnOff();
            }

            return true;
        }
    }

    public Product? GetProduct()
    {
        lock (_lock)
        {
            if (_products.Count == 0)
            {
                return null;
            }

            var product = _products.Pop();

            IsFull.TurnOff();

            if (_products.Count == 0)
            {
                IsEmpty.TurnOn();
            }
            else
            {
                IsEmpty.TurnOff();
            }

            if (_products.Count > 0 && _products.Count < _maxCapacity)
            {
                IsNotFullAndNotEmpty.TurnOn();
            }
            else
            {
                IsNotFullAndNotEmpty.TurnOff();
            }

            return product;
        }
    }
}