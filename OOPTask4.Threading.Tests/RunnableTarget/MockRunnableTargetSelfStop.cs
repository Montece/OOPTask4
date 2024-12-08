﻿namespace OOPTask4.Threading.Tests.RunnableTarget;

internal sealed class MockRunnableTargetSelfStop(RunnableContext context) : Threading.RunnableTarget(context, TimeSpan.Zero)
{
    protected override void RunInternal()
    {
        try
        {
            while (true)
            {
                (Context as MockRunnableCancellationTokenContext)!.CancellationToken.ThrowIfCancellationRequested();
            }
        }
        catch
        {
            // ignored
        }
    }
}