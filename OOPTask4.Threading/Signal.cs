namespace OOPTask4.Threading;

public sealed class Signal(bool initialState) : IDisposable
{
    private bool _disposed;

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

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _manualResetEvent.Dispose();
        }

        _disposed = true;
    }

    ~Signal()
    {
        Dispose(false);
    }
}