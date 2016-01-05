using System;
using System.Collections.Generic;

namespace Redcat.Core
{
    public static class ObserverExtensions
    {
        public static void OnCompleted<T>(this IEnumerable<IObserver<T>> observers)
        {
            foreach (IObserver<T> observer in observers) observer.OnCompleted();
        }

        public static void OnError<T>(this IEnumerable<IObserver<T>>observers, Exception error)
        {
            foreach (IObserver<T> observer in observers) observer.OnError(error);
        }

        public static void OnNext<T>(this IEnumerable<IObserver<T>> observers, T value)
        {
            foreach (IObserver<T> observer in observers) observer.OnNext(value);
        }

        public static IDisposable Subscribe<T>(this ICollection<IObserver<T>> observers, IObserver<T> observer)
        {
            observers.Add(observer);
            return new Subscription<T>(observers, observer);
        }
    }

    internal class Subscription<T> : IDisposable
    {
        private ICollection<IObserver<T>> observers;
        private IObserver<T> observer;

        internal Subscription(ICollection<IObserver<T>> observers, IObserver<T> observer)
        {
            this.observers = observers;
            this.observer = observer;
        }

        public void Dispose()
        {
            observers.Remove(observer);
        }
    }
}
