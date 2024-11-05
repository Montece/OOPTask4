namespace OOPTask4.Threading;

public sealed class RunnablesPool
{
    private List<Runnable> _runnables = new();

    public RunnablesPool()
    {

    }

    public void AddAndStart(RunnableTarget runnableTarget)
    {
        var runnable = new Runnable(runnableTarget);
        ru
    }
}