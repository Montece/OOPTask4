using OOPTask4.Threading.Tests.Runnable;

namespace OOPTask4.Threading.Tests.RunnableTarget;

internal sealed class MockRunnableTargetAffectToFlag(object context) : Threading.RunnableTarget(context, TimeSpan.Zero)
{
    protected override void RunInternal()
    {
        try
        {
            while (true)
            {
                (Context as MockRunnableFlagContext)!.Flag = true;
            }
        }
        catch
        {
            // ignored
        }
    }
}