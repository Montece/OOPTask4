using OOPTask4.Threading.Tests.RunnableTarget;
using Xunit;

namespace OOPTask4.Threading.Tests.Runnable;

public sealed class RunnableTests
{
    [Fact]
    public void Runnable_Ctor_Fail()
    {
        Assert.Throws<ArgumentNullException>(() => new Threading.Runnable(null!));
    }

    [Fact]
    public void Runnable_Ctor_Success()
    {
        var context = new MockRunnableFlagContext();
        var runnable = new Threading.Runnable(new MockRunnableTargetAffectToFlag(context));

        Assert.NotNull(runnable);
    }

    [Fact]
    public void Runnable_Start_Success()
    {
        var context = new MockRunnableFlagContext();
        var runnable = new Threading.Runnable(new MockRunnableTargetAffectToFlag(context));
        runnable.Start();
        context.Flag = false;
        Thread.Sleep(500);

        Assert.True(context.Flag, "Runnable didn't run!");
    }

    [Fact]
    public void Runnable_Stop_Success()
    {
        var context = new MockRunnableFlagContext();
        var runnable = new Threading.Runnable(new MockRunnableTargetAffectToFlag(context));
        runnable.Start();
        runnable.Stop();
        context.Flag = false;
        Thread.Sleep(500);

        Assert.False(context.Flag, "Runnable didn't stop!");
    }
}