using OOPTask4.Threading.Tests.Runnable;
using Xunit;

namespace OOPTask4.Threading.Tests.RunnableTarget;

public sealed class RunnableTargetTests
{
    [Fact]
    public void RunnableTarget_Ctor_Fail()
    {
        Assert.Throws<ArgumentNullException>(() => new MockRunnableTargetAffectToFlag(null!));
    }

    [Fact]
    public void RunnableTarget_Ctor_Success()
    {
        var context = new MockRunnableFlagContext();
        var runnableTarget = new MockRunnableTargetAffectToFlag(context);

        Assert.NotNull(runnableTarget);
    }

    [Fact]
    public void RunnableTarget_Run_Success()
    {
        var context = new MockRunnableCancellationTokenContext();
        var runnableTarget = new MockRunnableTargetAffectToFlag(context);
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        runnableTarget.Run(cancellationTokenSource.Token);
    }
}