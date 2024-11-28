namespace OOPTask4.Core.Products;

public abstract class UniqueObject
{
    public Guid Id { get; } = Guid.NewGuid();
}