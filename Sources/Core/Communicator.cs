using Redcat.Core.Services;
using System;

namespace Redcat.Core
{
    public class Communicator : CommandProcessor
    {
        public void Connect(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            if (!IsRunning) throw new InvalidOperationException();
            Service<IChannelManager>().OpenChannel(settings);
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

        public void Send(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
