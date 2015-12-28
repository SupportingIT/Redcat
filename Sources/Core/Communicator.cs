using Redcat.Core.Communication;
using System;

namespace Redcat.Core
{
    public class Communicator : ICommunicator, IDisposable
    {
        private bool initialized = false;

        private IChannelManager channelManager;
        private IMessageDispatcher messageDispatcher;

        public Communicator(IChannelManager channelManager, IMessageDispatcher messageDispatcher)
        {
            this.channelManager = channelManager;
            this.messageDispatcher = messageDispatcher;
        }

        protected bool IsRunning
        {
            get { return initialized; }
        }        

        public void Start()
        {
            if (initialized) return;
            OnBeforeInit();
            OnInit();
            OnAfterInit();
            initialized = true;
        }

        public void Connect(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            //if (!IsRunning) throw new InvalidOperationException("Start method must be called before opening any channels");
            channelManager.OpenChannel(settings);
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Send<T>(T message)
        {
            messageDispatcher.Dispatch(message);
        }

        public void Send(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            //if (!IsRunning) throw new InvalidOperationException("Run method must be called before sending any messages");
            messageDispatcher.Dispatch(message);
        }

        protected virtual void OnBeforeInit()
        { }

        protected virtual void OnInit()
        { }

        protected virtual void OnAfterInit()
        { }

        public void Dispose()
        { }
    }
}
