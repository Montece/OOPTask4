﻿namespace OOPTask4.Threading;

public abstract class RunnableTarget
{
    protected object Context { get; private set; }
    private TimeSpan Period { get; }
    public bool IsPaused { get; set; }

    protected RunnableTarget(object context, TimeSpan period)
    {
        ArgumentNullException.ThrowIfNull(context);

        Period = period;
        Context = context;
    }

    public void Run(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (!IsPaused)
            {
                RunInternal();
            }

            Thread.Sleep(Period);
        }
    }

    protected abstract void RunInternal();
}