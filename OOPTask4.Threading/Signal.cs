namespace OOPTask4.Threading;

public sealed class Signal(bool initialState)
{
    private readonly ManualResetEvent _manualResetEvent = new(initialState);

    public void TurnOn()
    {
        _manualResetEvent.Set();
    }

    public void TurnOff()
    {
        _manualResetEvent.Reset();
    }

    public void WaitForTurnOn()
    {
        _manualResetEvent.WaitOne();
    }

    public void WaitForTurnOn(TimeSpan timeout)
    {
        _manualResetEvent.WaitOne(timeout);
    }
}