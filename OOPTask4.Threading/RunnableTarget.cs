namespace OOPTask4.Threading;

public abstract class RunnableTarget
{
    public void Run(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            RunInternal();
        }
    }

    protected abstract void RunInternal();
}