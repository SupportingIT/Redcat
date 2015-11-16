using System;

namespace Redcat.Core.Communication
{
    public interface IChannel
    {
        void Open();
        void Close();
        ChannelState State { get; }

        event EventHandler<StateChangedEventArgs> StateChanged;
    }

    public interface IChannel<T> : IChannel
    {
        void Send(T message);
        T Receive();
    }

    public enum ChannelState { Open, Opening, Close, Closing }

    public class StateChangedEventArgs : EventArgs
    {
        public StateChangedEventArgs(ChannelState newState)
        {
            NewState = newState;
        }

        public ChannelState NewState { get; private set; }
    }
}
