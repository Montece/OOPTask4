using OOPTask4.Core.Products;
using OOPTask4.Core.Warehouse;
using Xunit;

namespace OOPTask4.Core.Tests;

public class WarehouseTests
{
    [Fact]
    public void Warehouse_Ctor_Success()
    {
        var supplier = new Warehouse<Engine>(100);

        Assert.NotNull(supplier);
    }

    [Theory]
    [InlineData(-5)]
    [InlineData(0)]
    public void Warehouse_Ctor_Fail(int capacity)
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentException>(() => { new Warehouse<Engine>(capacity); });
    }

    [Fact]
    public void Warehouse_AddProduct_Increased()
    {
        var supplier = new Warehouse<MockProduct>(100);
        var product = new MockProduct();
        var addResult = supplier.AddProduct(product);
        addResult &= supplier.AddProduct(product);
        addResult &= supplier.AddProduct(product);
        
        Assert.True(addResult, "Cannot add 3 products");
        Assert.Equal(3, supplier.ProductsCount);
    }
}