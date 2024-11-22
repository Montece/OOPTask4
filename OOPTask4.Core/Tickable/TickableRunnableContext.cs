using OOPTask4.Threading;

namespace OOPTask4.Core.Tickable;

public sealed class TickableRunnableContext : RunnableContext
{
    public ITickable Tickable { get; }

    public TickableRunnableContext(ITickable tickable)
    {
        ArgumentNullException.ThrowIfNull(tickable);

        Tickable = tickable;
    }
}