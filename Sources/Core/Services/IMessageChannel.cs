using System;

namespace Redcat.Core
{
    public interface IMessageChannel
    {
        void Open();
        void Close();
        void Send(Message message);
        Message Receive();

        ChannelState State { get; }

        event EventHandler<StateChangedEventArgs> StateChanged;
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
