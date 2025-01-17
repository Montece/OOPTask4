using OOPTask4.Core.Products;
using OOPTask4.Core.Supply;
using OOPTask4.Core.Warehouse;
using Xunit;

namespace OOPTask4.Core.Tests;

public sealed class SupplierTests
{
    [Fact]
    public void Supplier_Ctor_Success()
    {
        using var targetWarehouse = new Warehouse<Engine>(10);
        var supplier = new Supplier<Engine>(targetWarehouse);

        Assert.NotNull(supplier);
    }

    [Fact]
    public void Supplier_TickWithWarehouse_Success()
    {
        using var targetWarehouse = new Warehouse<Engine>(10);
        var supplier = new Supplier<Engine>(targetWarehouse);
        supplier.Tick();
    }

    [Fact]
    public void Supplier_TickWithEmptyWarehouse_Success()
    {
        using var warehouse = new Warehouse<Engine>(1);
        var supplier = new Supplier<Engine>(warehouse);

        var oldWarehouseComponentsCount = warehouse.ProductsCount;
        supplier.Tick();
        var newWarehouseComponentsCount = warehouse.ProductsCount;

        Assert.Equal(oldWarehouseComponentsCount, newWarehouseComponentsCount - 1);
    }
}