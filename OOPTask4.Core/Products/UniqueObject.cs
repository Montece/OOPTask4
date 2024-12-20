namespace OOPTask4.Core.Products;

public abstract class UniqueObject
{
    private Guid Id { get; } = Guid.NewGuid();

    public override string ToString()
    {
        return $"{GetType().Name} {Id}";
    }
}