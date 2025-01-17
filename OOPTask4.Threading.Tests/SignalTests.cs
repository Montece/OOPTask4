using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace OOPTask4.Threading.Tests;

[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
public sealed class SignalTests
{
    [Fact]
    public void Signal_Ctor_Success()
    {
        using var signal0 = new Signal(true);
        using var signal1 = new Signal(false);

        Assert.NotNull(signal0);
        Assert.NotNull(signal1);
    }

    [Fact]
    public async Task Signal_TurnOnAndWait_CorrectOrder()
    {
        var results = new List<string>();
        var beforeText = "before signal";
        var afterText = "after signal";

        using var signal0 = new Signal(false);

        var task0 = Task.Run(() =>
        {
            signal0.WaitForTurnOn();
            results.Add(afterText);
        });

        var task1 = Task.Run(() =>
        {
            results.Add(beforeText);
            signal0.TurnOn();
        });

        await task0;
        await task1;

        Assert.Equal(results[0], beforeText);
        Assert.Equal(results[1], afterText);
    }

    [Fact]
    public async Task Signal_TurnOffAndWait_CorrectOrder()
    {
        var results = new List<string>();
        var beforeText = "before signal";
        var afterText = "after signal";

        using var signal0 = new Signal(true);

        var task0 = Task.Run(() =>
        {
            signal0.TurnOff();
            signal0.WaitForTurnOn();
            results.Add(afterText);
        });

        var task1 = Task.Run(() =>
        {
            results.Add(beforeText);
            signal0.TurnOn();
        });

        await task0;
        await task1;

        Assert.Equal(results[0], beforeText);
        Assert.Equal(results[1], afterText);
    }
        
    [Fact]
    public void Signal_WaitForTurnOn_Timeout()
    {
        var timeoutTimeMs = 100;
        using var signal = new Signal(false);
        signal.WaitForTurnOn(TimeSpan.FromMilliseconds(timeoutTimeMs));
    }
}