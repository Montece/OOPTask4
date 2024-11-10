namespace OOPTask4.Threading;

public sealed class RunnablesPool
{
    private readonly List<Runnable> _runnables = new();

    public Runnable AddAndStart(RunnableTarget runnableTarget)
    {
        ArgumentNullException.ThrowIfNull(runnableTarget);

        var runnable = new Runnable(runnableTarget);

        runnable.Start();

        _runnables.Add(runnable);

        return runnable;
    }

    public bool RemoveAndStop(Runnable runnable)
    {
        ArgumentNullException.ThrowIfNull(runnable);

        if (!_runnables.Contains(runnable))
        {
            return false;
        }

        runnable.Stop();

        _runnables.Remove(runnable);
        
        return true;
    }
}