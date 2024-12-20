using OOPTask4.Threading.Tickable;

namespace OOPTask4.Threading;

public sealed class RunnersPool : IDisposable
{
    private readonly Queue<ITickable> _tickablesQueue = new();
    private readonly List<Runner> _allRunners = [];
    private readonly List<Runner> _freeRunners = [];

    private readonly object _lock = new();

    private Signal HasFreeRunners { get; } = new(true);
    private Signal TickablesAreWaiting { get; } = new(false);

    private bool _isRunning = true;

    public RunnersPool(int runnersCount)
    {
        for (var i = 0; i < runnersCount; i++)
        {
            var runner = new Runner(StopRunnerReaction);
            _allRunners.Add(runner);
            _freeRunners.Add(runner);
        }

        new Thread(ProcessPoolLogic).Start();
    }

    private void ProcessPoolLogic()
    {
        while (_isRunning)
        {
            ITickable? tickable = null;

            do
            {
                TickablesAreWaiting.WaitForTurnOn();

                lock (_lock)
                {
                    if (_tickablesQueue.Count > 0)
                    {
                        tickable = _tickablesQueue.Dequeue();
                    }
                }
            }
            while (tickable == null);

            lock (_lock)
            {
                if (_tickablesQueue.Count == 0)
                {
                    TickablesAreWaiting.TurnOff();
                }
            }

            Runner? runner;

            do
            {
                HasFreeRunners.WaitForTurnOn();

                lock (_lock)
                {
                    runner = _freeRunners.FirstOrDefault();
                }
            }
            while (runner == null);

            lock (_lock)
            {
                _freeRunners.Remove(runner);
            }
        
            runner.DoTick(tickable);

            lock (_lock)
            {
                if (_freeRunners.Count == 0)
                {
                    HasFreeRunners.TurnOff();
                }
            }
        }
    }

    public void DoTickOnTarget(ITickable tickable)
    {
        ArgumentNullException.ThrowIfNull(tickable);

        lock (_lock)
        {
            _tickablesQueue.Enqueue(tickable);
            TickablesAreWaiting.TurnOn();
        }
    }

    private void StopRunnerReaction(Runner runner)
    {
        ArgumentNullException.ThrowIfNull(runner);

        lock (_lock)
        {
            _freeRunners.Add(runner);
            HasFreeRunners.TurnOn();
        }
    }

    public void Dispose()
    {
        _isRunning = false;

        lock (_lock)
        {
            foreach (var runner in _allRunners)
            {
                runner.Dispose();
            }
        }

        HasFreeRunners.Dispose();
        TickablesAreWaiting.Dispose();
    }
}