using OOPTask4.Core.Products;
using OOPTask4.Core.Supplier;
using OOPTask4.Core.Warehouse;
using Xunit;

namespace OOPTask4.Core.Tests;

public sealed class SupplierTests
{
    [Fact]
    public void Supplier_Ctor_Success()
    {
        var supplier = new Supplier<Engine>();

        Assert.NotNull(supplier);
    }

    [Fact]
    public void Supplier_TickWithoutWarehouse_Exception()
    {
        Assert.Throws<NotSpecifiedWarehouseException>(() =>
        {
            var supplier = new Supplier<Engine>();
            supplier.Tick();
        });
    }

    [Fact]
    public void Supplier_TickWithEmptyWarehouse_Success()
    {
        var supplier = new Supplier<Engine>();
        var warehouse = new Warehouse<Engine>(1);
        supplier.BindToWarehouse(warehouse);

        var oldWarehouseComponentsCount = warehouse.ProductsCount;
        supplier.Tick();
        var newWarehouseComponentsCount = warehouse.ProductsCount;

        Assert.Equal(oldWarehouseComponentsCount, newWarehouseComponentsCount - 1);
    }

    [Fact]
    public void Supplier_TickWithFullWarehouse_Exception()
    {
        Assert.Throws<WarehouseIsFullException>(() =>
        {
            var supplier = new Supplier<Engine>();
            var warehouse = new Warehouse<Engine>(1);
            supplier.BindToWarehouse(warehouse);

            supplier.Tick();
            supplier.Tick();
        });
    }
}