using System;
using System.Threading;
using UniRx;

namespace Code.Common.Async
{
    public sealed class ReactiveWaiter : IReactiveWaiter
    {
        public IDisposable WaitBool<T>(
            Func<T, bool> checkFunc,
            T arg,
            bool checkFlag,
            Action onReached = null,
            TimeSpan? period = null,
            CancellationToken token = default)
        {
            var sub = WaitBoolAsObservable(checkFunc, arg, checkFlag, period)
                .Subscribe(_ => onReached?.Invoke());

            if (token.CanBeCanceled)
                token.Register(() => sub.Dispose());

            return sub;
        }

        public IDisposable WaitBool(
            Func<bool> checkFunc,
            bool checkFlag,
            Action onReached = null,
            TimeSpan? period = null,
            CancellationToken token = default)
        {
            var sub = WaitBoolAsObservable(checkFunc, checkFlag, period)
                .Subscribe(_ => onReached?.Invoke());

            if (token.CanBeCanceled)
                token.Register(() => sub.Dispose());

            return sub;
        }

        public IObservable<Unit> WaitBoolAsObservable<T>(
            Func<T, bool> checkFunc,
            T arg,
            bool checkFlag,
            TimeSpan? period = null)
        {
            // Тик: либо каждый кадр, либо по периоду (на главном потоке)
            var tick = period == null
                ? Observable.EveryUpdate()
                : Observable.Timer(TimeSpan.Zero, period.Value, Scheduler.MainThread);

            return tick
                .Select(_ => checkFunc(arg))
                .DistinctUntilChanged()          // защиты от лишних вызовов
                .Where(v => v == checkFlag)
                .Take(1)
                .AsUnitObservable();
        }

        public IObservable<Unit> WaitBoolAsObservable(
            Func<bool> checkFunc,
            bool checkFlag,
            TimeSpan? period = null)
        {
            var tick = period == null
                ? Observable.EveryUpdate()
                : Observable.Timer(TimeSpan.Zero, period.Value, Scheduler.MainThread);

            return tick
                .Select(_ => checkFunc())
                .DistinctUntilChanged()
                .Where(v => v == checkFlag)
                .Take(1)
                .AsUnitObservable();
        }
    }
}
