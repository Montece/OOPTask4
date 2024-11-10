namespace OOPTask4.Threading;

public abstract class RunnableTarget
{
    protected RunnableContext Context { get; private set; }

    protected RunnableTarget(RunnableContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        Context = context;
    }

    public void Run(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            RunInternal();
        }
    }

    protected abstract void RunInternal();
}