using OOPTask4.Threading.Tickable;

namespace OOPTask4.Threading.Tests.Runner;

internal sealed class TickableMock : ITickable
{
    public int TicksCount { get; private set; }

    public object Clone()
    {
        return new TickableMock();
    }

    public void Tick()
    {
        TicksCount++;
    }
}