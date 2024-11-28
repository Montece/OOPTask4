using OOPTask4.Threading;

namespace OOPTask4.Core.Tickable;

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
            // TODO Показ ошибки? Или запись в лог
        }
    }

    public override string ToString()
    {
        return context.Tickable.ToString()!;
    }
}