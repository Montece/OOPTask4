using OOPTask4.Core.Tickable;
using OOPTask4.Threading;

namespace OOPTask4.Core;

public class TickableGroup<T> where T : ITickable
{
    private readonly RunnablesPool _pool = new();

    public Runnable Add(T tickable)
    {
        var runnable = _pool.AddAndStart(new TickableRunnableTarget(new(tickable), TimeSpan.FromMilliseconds(1)));
        return runnable;
    }

    public void Remove(Runnable runnable)
    {
        _pool.RemoveAndStop(runnable);
    }

    public IReadOnlyCollection<Runnable> GetTickables()
    {
        return _pool.GetRunnables();
    }

    /* Dealer => CarWarehouse.RequestCar() => WorkManager.RequestCar() => {
    {
        Пока не произведется N машин то работают все воркеры


        // Система слипов. Мы не работаем, пока не требуется.
        // Пока не требуются машины, мы их не производим
    }

    */
}