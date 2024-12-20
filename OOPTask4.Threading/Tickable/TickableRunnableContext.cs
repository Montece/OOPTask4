namespace OOPTask4.Threading.Tickable;

public sealed class TickableRunnableContext
{
    public ITickable Tickable { get; }

    public TickableRunnableContext(ITickable tickable)
    {
        ArgumentNullException.ThrowIfNull(tickable);

        Tickable = tickable;
    }
}