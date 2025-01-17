using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace OOPTask4.Threading.Tests;

[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
public sealed class SignalTests
{
    [Fact]
    public void Signal_Ctor_Success()
    {
        using var signalOn = new Signal(true);
        using var signalOff = new Signal(false);
    }

    [Fact]
    public async Task Signal_TurnOnAndWait_CorrectOrder()
    {
        var results = new List<string>();
        var textBefore = "before signal";
        var textAfter = "after signal";

        using var signal = new Signal(false);

        var taskAfter = Task.Run(() =>
        {
            signal.WaitForTurnOn();
            results.Add(textAfter);
        });

        var taskBefore = Task.Run(() =>
        {
            results.Add(textBefore);
            signal.TurnOn();
        });

        await taskAfter;
        await taskBefore;

        Assert.Equal(results[0], textBefore);
        Assert.Equal(results[1], textAfter);
    }

    [Fact]
    public async Task Signal_TurnOffAndWait_CorrectOrder()
    {
        var results = new List<string>();
        var textBefore = "before signal";
        var textAfter = "after signal";

        using var signal = new Signal(true);

        var taskAfter = Task.Run(() =>
        {
            signal.TurnOff();
            signal.WaitForTurnOn();
            results.Add(textAfter);
        });

        var taskBefore = Task.Run(() =>
        {
            results.Add(textBefore);
            signal.TurnOn();
        });

        await taskAfter;
        await taskBefore;

        Assert.Equal(results[0], textBefore);
        Assert.Equal(results[1], textAfter);
    }
        
    [Fact]
    public void Signal_WaitForTurnOn_Timeout()
    {
        var timeoutTimeMs = 100;
        using var signal = new Signal(false);
        signal.WaitForTurnOn(TimeSpan.FromMilliseconds(timeoutTimeMs));
    }
}