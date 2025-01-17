using NLog;
using OOPTask4.Threading.Tickable;

namespace OOPTask4.Threading;

public sealed class TickableThreadPool
{
    private bool _isRunning = true;

    private readonly object _isRunningLock = new();
    private readonly object _tickablesLock = new();
    private readonly TimeSpan _waitTimeout = TimeSpan.FromMilliseconds(100);
    private readonly Queue<ITickable> _tickablesQueue = new();

    public TickableThreadPool(int threadsCount)
    {
        if (threadsCount <= 0)
        {
            throw new ArgumentException("Must be greater than zero", nameof(threadsCount));
        }

        for (var i = 0; i < threadsCount; i++)
        {
            var thread = new Thread(ProcessThreadLogic);
            thread.Start();
        }
    }

    private void ProcessThreadLogic()
    {
        while (true)
        {
            try
            {
                lock (_isRunningLock)
                {
                    if (!_isRunning)
                    {
                        return;
                    }
                }

                ITickable? tickable = null;

                lock (_tickablesLock)
                {
                    Monitor.Wait(_tickablesLock, _waitTimeout);

                    if (_tickablesQueue.Count > 0)
                    {
                        tickable = _tickablesQueue.Dequeue();
                    }
                }

                tickable?.Tick();
            }
            catch (Exception ex)
            {
                Logger.NLogger.Log(LogLevel.Error, ex);
            }
        }
    }

    public void DoTick(ITickable tickable, int ticksCount = 1)
    {
        ArgumentNullException.ThrowIfNull(tickable);

        lock (_tickablesLock)
        {
            for (var i = 0; i < ticksCount; i++)
            {
                _tickablesQueue.Enqueue(tickable);
            }

            Monitor.PulseAll(_tickablesLock);
        }
    }

    ~TickableThreadPool()
    {
        lock (_isRunningLock)
        {
            _isRunning = false;
        }
    }
}