using System;

namespace Redcat.Core
{
    public abstract class MessageChannelBase : IMessageChannel
    {
        private ConnectionSettings settings;
        private ChannelState state;

        protected MessageChannelBase()
        {
            state = ChannelState.Close;
        }

        public ChannelState State
        {
            get { return state; }
        }

        public void Open()
        {
            OnOpening();
            OnOpen();            
        }

        protected virtual void OnOpening()
        {
            ChangeState(ChannelState.Opening);
        }

        protected virtual void OnOpen()
        {
            ChangeState(ChannelState.Open);
        }

        public void Close()
        {
            OnClosing();
            OnClose();
        }

        protected virtual void OnClosing()
        {
            ChangeState(ChannelState.Closing);
        }

        protected virtual void OnClose()
        {
            ChangeState(ChannelState.Close);
        }

        private void ChangeState(ChannelState newState)
        {
            state = newState;
            OnStateChanged(newState);
        }

        protected virtual void OnStateChanged(ChannelState state)
        {
            if (StateChanged != null) StateChanged(this, new StateChangedEventArgs(state));
        }

        public abstract void Send(Message message);

        public abstract Message Receive();

        public event EventHandler<StateChangedEventArgs> StateChanged;
    }
}
