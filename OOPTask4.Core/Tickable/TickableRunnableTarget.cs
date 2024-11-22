using OOPTask4.Threading;

namespace OOPTask4.Core.Tickable;

public sealed class TickableRunnableTarget(TickableRunnableContext context, TimeSpan period) : RunnableTarget(context, period)
{
    protected override void RunInternal()
    {
        try
        {
            context.Tickable.Tick();
        }
        catch (Exception ex)
        {
            // TODO Показ ошибки? Или запись в лог
        }
    }
}