using OOPTask4.Threading.Tickable;

namespace OOPTask4.Threading;

internal sealed class Runner : IDisposable
{
    private bool IsAlive { get; set; }
    private Signal IsExecuting { get; } = new(false);

    private readonly Action<Runner> _stopReaction;
    private readonly Thread _thread;
    private ITickable? _tickable;

    public Runner(Action<Runner> stopReaction)
    {
        _stopReaction = stopReaction;
        _thread = new(ExecuteCycle);

        Alive();
    }

    private void Alive()
    {
        if (IsAlive)
        {
            return;
        }

        _thread.Start();
        IsAlive = true;
    }

    public void DoTick(ITickable tickable)
    {
        _tickable = tickable;
        IsExecuting.TurnOn();
    }

    private void ExecuteCycle()
    {
        while (IsAlive)
        {
            if (_tickable != null)
            {
                _tickable.Tick();
                _tickable = null;
                IsExecuting.TurnOff();
                _stopReaction?.Invoke(this);
            }

            IsExecuting.WaitForTurnOn();
        }
    }

    public void Dispose()
    {
        IsExecuting.Dispose();
    }
}