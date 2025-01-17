namespace OOPTask4.Threading.Tickable;

public sealed class TickableGroup<T> where T : class, ITickable
{
    private readonly TickableThreadPool _pool;
    private readonly List<T> _tickables = [];

    public TickableGroup(TickableThreadPool pool, T tickableToClone, int tickablesCount)
    {
        ArgumentNullException.ThrowIfNull(pool);
        ArgumentNullException.ThrowIfNull(tickableToClone);

        if (tickablesCount <= 0)
        {
            throw new ArgumentException("Must be greater than zero", nameof(tickablesCount));
        }

        _pool = pool;

        for (var i = 0; i < tickablesCount; i++)
        {
            if (tickableToClone.Clone() is T newTickable)
            {
                _tickables.Add(newTickable);
            }
        }
    }

    public IReadOnlyList<T> GetTickables()
    {
        return _tickables;
    }

    public void DoWork()
    {
        foreach (var worker in _tickables)
        {
            _pool.DoTick(worker, ticksCount: 1);
        }
    }
}