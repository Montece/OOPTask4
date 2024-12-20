namespace OOPTask4.Threading.Tickable;

public sealed class TickableRunnableTarget(TickableRunnableContext context, TimeSpan period, long? maximumTicksCount = null) : RunnableTarget(context, period)
{
    private long _currentTicksCount;

    protected override void RunInternal()
    {
        if (maximumTicksCount.HasValue)
        {
            if (_currentTicksCount >= maximumTicksCount)
            {
                return;
            }

            _currentTicksCount++;
        }

        try
        {
            context.Tickable.Tick();
        }
        catch (Exception ex)
        {
            Logger.NLogger.Error(ex);
        }
    }

    public override string ToString()
    {
        return context.Tickable.ToString()!;
    }
}