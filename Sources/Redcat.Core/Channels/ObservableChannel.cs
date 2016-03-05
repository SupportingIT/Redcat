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

        protected virtual void OnMessageReceived(T message)
        {
            MessageReceived?.Invoke(this, new ChannelMessageEventArgs<T>(message));
        }

        protected virtual void OnMessageSended(T message)
        {
            MessageSended?.Invoke(this, new ChannelMessageEventArgs<T>(message));
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return observers.Subscribe(observer);
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            observers.OnCompleted();
            RemoveSubscribers(MessageReceived);
            RemoveSubscribers(MessageSended);
        }

        public event EventHandler<ChannelMessageEventArgs<T>> MessageReceived;
        public event EventHandler<ChannelMessageEventArgs<T>> MessageSended;
    }

    public class ChannelMessageEventArgs<T> : EventArgs
    {
        public ChannelMessageEventArgs(T message)
        {
            Message = message;
        }

        public T Message { get; }
    }
}
