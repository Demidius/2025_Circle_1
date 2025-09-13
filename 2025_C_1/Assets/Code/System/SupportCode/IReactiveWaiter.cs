using System;
using System.Threading;
using UniRx;

namespace Code.Common.Async
{
    public interface IReactiveWaiter
    {
        // Вернёт подписку; отмена — Dispose или token.Cancel()
        IDisposable WaitBool<T>(
            Func<T, bool> checkFunc,
            T arg,
            bool checkFlag,
            Action onReached = null,
            TimeSpan? period = null,
            CancellationToken token = default);

        // Тоже самое, но без аргумента
        IDisposable WaitBool(
            Func<bool> checkFunc,
            bool checkFlag,
            Action onReached = null,
            TimeSpan? period = null,
            CancellationToken token = default);

        // Для RX-композиции: дождись и эмитьни Unit
        IObservable<Unit> WaitBoolAsObservable<T>(
            Func<T, bool> checkFunc,
            T arg,
            bool checkFlag,
            TimeSpan? period = null);

        IObservable<Unit> WaitBoolAsObservable(
            Func<bool> checkFunc,
            bool checkFlag,
            TimeSpan? period = null);
    }
}
