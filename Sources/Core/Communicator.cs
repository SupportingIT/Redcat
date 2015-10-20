using Redcat.Core.Services;
using System;

namespace Redcat.Core
{
    public class Communicator : CommandProcessor
    {
        private IChannelManager channelManager;
        private IMessageDispatcher messageDispatcher;

        protected IChannelManager ChannelManager
        {
            get { return channelManager; }
        }

        protected IMessageChannel DefaultChannel
        {
            get { return ChannelManager.DefaultChannel; }
        }

        protected IMessageDispatcher MessageDispatcher
        {
            get { return messageDispatcher; }
        }

        public void Connect(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            if (!IsRunning) throw new InvalidOperationException();
            ChannelManager.OpenChannel(settings);
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        protected override void OnAfterInit()
        {
            base.OnAfterInit();            
        }

        public void Send(Message message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (DefaultChannel == null) throw new InvalidOperationException("No active connections");
            throw new NotImplementedException();
        }
    }
}
