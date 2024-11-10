namespace OOPTask4.Threading.Tests;

internal sealed class MockRunnableTargetAffectToFlag(RunnableContext context) : RunnableTarget(context)
{
    protected override void RunInternal()
    {
        try
        {
            while (true)
            {
                (Context as MockRunnableContext)!.Flag = true;
            }
        }
        catch
        {
            // ignored
        }
    }
}