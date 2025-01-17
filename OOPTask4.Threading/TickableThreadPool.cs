using NLog;
using OOPTask4.Threading.Tickable;

namespace OOPTask4.Threading;

public sealed class TickableThreadPool
{
    private bool _isRunning = true;

    private readonly object _isRunningLock = new();
    private readonly object _tickablesLock = new();
    private readonly Queue<ITickable> _tickablesQueue = new();
    private static readonly TimeSpan waitTimeout = TimeSpan.FromMilliseconds(100);

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
            ITickable? tickable = null;

            try
            {
                lock (_isRunningLock)
                {
                    if (!_isRunning)
                    {
                        return;
                    }
                }

                lock (_tickablesLock)
                {
                    Monitor.Wait(_tickablesLock, waitTimeout);

                    if (_tickablesQueue.Count > 0)
                    {
                        tickable = _tickablesQueue.Dequeue();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.NLogger.Log(LogLevel.Error, ex, "Error in pool!");
            }

            try
            {
                tickable?.Tick();
            }
            catch (Exception ex)
            {
                Logger.NLogger.Log(LogLevel.Error, ex, "Error in tick!");
            }
        }
    }

    public void DoTick(ITickable tickable, int ticksCount)
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
}