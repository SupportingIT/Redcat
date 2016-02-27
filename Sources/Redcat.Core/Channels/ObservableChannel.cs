using System;
using System.Collections.Generic;

namespace Redcat.Core.Channels
{
    public class ObservableChannel<T> : ChannelBase, IObservable<T>
    {
        private ICollection<IObserver<T>> observers;

        protected ObservableChannel(ConnectionSettings settings) : base(settings)
        {            
            observers = new List<IObserver<T>>();
        }

        protected void RiseOnCompleted() => observers.OnCompleted();

        protected void RiseOnNext(T message) => observers.OnNext(message);

        protected void RiseOnError(Exception error) => observers.OnError(error);

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return observers.Subscribe(observer);
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            observers.OnCompleted();
        }
    }
}
