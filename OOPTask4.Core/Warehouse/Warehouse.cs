using JetBrains.Annotations;
using OOPTask4.Core.Products;
using OOPTask4.Threading;

namespace OOPTask4.Core.Warehouse;

public sealed class Warehouse<T> : IDisposable where T : Product
{
    private Signal IsFull { get; } = new(false);
    public Signal IsNotFullAndNotEmpty { get; } = new(false);
    private Signal IsEmpty { get; } = new(true);
    
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

    private bool _disposed;
    private readonly Stack<T> _products = [];
    private readonly object _lock = new();
    private readonly int _maxCapacity;

    public Warehouse(int maxCapacity)
    {
        Utility.CheckIfGreaterThanZero(maxCapacity, nameof(maxCapacity));

        _maxCapacity = maxCapacity;
    }

    public bool AddProduct(T product)
    {
        if (_disposed)
        {
            throw new InvalidOperationException("Object was disposed!");
        }

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

    [MustUseReturnValue]
    public Product? GetProduct()
    {
        if (_disposed)
        {
            throw new InvalidOperationException("Object was disposed!");
        }

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

    [MustUseReturnValue]
    public IReadOnlyList<T> GetProducts()
    {
        if (_disposed)
        {
            throw new InvalidOperationException("Object was disposed!");
        }

        lock (_lock)
        {
            return _products.ToList();
        }
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
            IsFull.Dispose();
            IsNotFullAndNotEmpty.Dispose();
            IsEmpty.Dispose();
        }

        _disposed = true;
    }

    ~Warehouse()
    {
        Dispose(false);
    }
}