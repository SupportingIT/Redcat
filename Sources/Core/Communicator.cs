using Redcat.Core.Services;
using System;

namespace Redcat.Core
{
    public class Communicator : CommandProcessor
    {
        private Lazy<IChannelManager> channelManager;
        private Lazy<IMessageDispatcher> messageDispatcher;

        public Communicator()
        {
            channelManager = new Lazy<IChannelManager>(GetService<IChannelManager>);
            messageDispatcher = new Lazy<IMessageDispatcher>(GetService<IMessageDispatcher>);
        }

        protected IChannelManager ChannelManager
        {
            get { return channelManager.Value; }
        }

        protected IMessageChannel DefaultChannel
        {
            get { return ChannelManager.DefaultChannel; }
        }

        protected IMessageDispatcher MessageDispatcher
        {
            get { return messageDispatcher.Value; }
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

        public void Send(Message message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (DefaultChannel == null) throw new InvalidOperationException("No active connections");
            throw new NotImplementedException();
        }

        protected override void OnBeforeInit()
        {
            base.OnBeforeInit();
            AddExtension("Redcat.Communicator", CommunicatorExtension);
        }

        private void CommunicatorExtension(IServiceContainer container)
        {
            IChannelManager channelManager = new ChannelManager(GetServices<IChannelFactory>);
            container.Add<IChannelManager>(channelManager);
        }
    }
}
