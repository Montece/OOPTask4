using Xunit;

namespace OOPTask4.Threading.Tests.Runner;

public sealed class RunnerTests
{
    [Fact]
    public void Runnable_DoTick_Success()
    {
        var runnable = new Threading.Runner(_ => {});
        var tickable = new TickableMock();
        var expectedTicksCount = 10;

        for (var i = 0; i < expectedTicksCount; i++)
        {
            runnable.DoTick(tickable);
            Thread.Sleep(50);
        }

        Assert.Equal(expectedTicksCount, tickable.TicksCount);
    }
}