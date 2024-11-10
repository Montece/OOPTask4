using OOPTask4.Threading.Tests.Runnalbe;

namespace OOPTask4.Threading.Tests.RunnableTarget;

internal sealed class MockRunnableTargetAffectToFlag(RunnableContext context) : Threading.RunnableTarget(context)
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