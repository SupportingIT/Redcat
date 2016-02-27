using System;

namespace Redcat.Core.Channels
{
    public abstract class ChannelBase : DisposableObject, IChannel
    {
        private ConnectionSettings settings;
        private ChannelState state;        

        protected ChannelBase(ConnectionSettings settings)
        {            
            state = ChannelState.Close;
            this.settings = settings;
        }

        protected ChannelBase()
        {
            state = ChannelState.Close;
        }

        protected ConnectionSettings Settings
        {
            get { return settings; }
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

        protected override void DisposeManagedResources()
        {
            Close();
        }

        public event EventHandler<StateChangedEventArgs> StateChanged;
    }
}
