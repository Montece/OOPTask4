using Xunit;

namespace OOPTask4.Threading.Tests;

public sealed class TickableThreadPoolTests
{
    [Fact]
    public void TickableThreadPool_Ctor_Success()
    {
        _ = new TickableThreadPool(10);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void TickableThreadPool_Ctor_Wrong_Count(int threadsCount)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new TickableThreadPool(threadsCount);
        });
    }

    [Fact]
    public void TickableThreadPool_DoTick_MatchExecuteCount()
    {
        var pool = new TickableThreadPool(10);
        var tickables = new List<TickableMock>();
        var tickablesCount = 10;
        var ticksForTickableCount = 3;

        for (var i = 0; i < tickablesCount; i++)
        {
            tickables.Add(new());
        }

        foreach (var tickable in tickables)
        {
            for (var i = 0; i < ticksForTickableCount; i++)
            {
                pool.DoTick(tickable, ticksCount: 1);
                Thread.Sleep(30);
            }
        }

        Thread.Sleep(75);

        var ticksSum = tickables.Sum(t => t.TicksCount);

        Assert.Equal(tickablesCount * ticksForTickableCount, ticksSum);
    }
}