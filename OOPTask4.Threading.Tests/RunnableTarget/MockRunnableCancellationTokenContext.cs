namespace OOPTask4.Threading.Tests.RunnableTarget;

internal sealed class MockRunnableCancellationTokenContext : RunnableContext
{
    public CancellationToken CancellationToken { get; set; }
}