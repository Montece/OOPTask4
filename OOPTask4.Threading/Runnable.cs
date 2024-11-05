namespace OOPTask4.Threading;

public sealed class Runnable
{
    private readonly RunnableTarget _runnableTarget;
    private readonly Thread _thread;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public Runnable(RunnableTarget runnableTarget)
    {
        ArgumentNullException.ThrowIfNull(runnableTarget);

        _runnableTarget = runnableTarget;
        _thread = new(() => _runnableTarget.Run(_cancellationTokenSource.Token));
    }

    public void Start()
    {
        if (_thread.IsAlive)
        {
            return;
        }

        _thread.Start();
    }

    public void Stop()
    {
        if (!_thread.IsAlive)
        {
            return;
        }

        _cancellationTokenSource.Cancel();
    }
}