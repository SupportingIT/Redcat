using Redcat.Core.Services;
using System;

namespace Redcat.Core
{
    public class Communicator : CommandProcessor
    {
        private IChannelManager channelManager;

        protected IChannelManager ChannelManager
        {
            get { return channelManager; }
        }

        protected IMessageChannel DefaultChannel
        {
            get { return ChannelManager.DefaultChannel; }
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

        protected override void OnBeforeInit()
        {
            base.OnBeforeInit();
            Kernel.Providers.Add(new CommunicatorServiceProvider(Kernel));
        }

        protected override void OnAfterInit()
        {
            base.OnAfterInit();
            channelManager = Service<IChannelManager>();
            if (channelManager == null) throw new InitializationException("Unable to find IChannelManager service which is required");
        }

        public void Send(Message message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (DefaultChannel == null) throw new InvalidOperationException("No active connections");
            DefaultChannel.Send(message);
        }
    }
}
