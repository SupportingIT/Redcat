using System;
using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Xmpp.Negotiators;

namespace Redcat.Xmpp.Services
{
    public class XmppChannel : NetworkChannel
    {
        private IStreamInitializer streamInitializer;
        private IXmppStream stream;

        public XmppChannel(IStreamInitializer streamInitializer, INetworkStreamFactory factory, ConnectionSettings settings) : base(factory, settings)
        {
            if (streamInitializer == null) throw new ArgumentNullException(nameof(streamInitializer));
            this.streamInitializer = streamInitializer;
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            stream = CreateXmppStream();
            streamInitializer.Init(stream);
        }

        protected virtual IXmppStream CreateXmppStream()
        {
            return new XmppStream(Stream);
        }

        private void ResetXmppStream()
        {
            stream = CreateXmppStream();
        }

        public override Message Receive()
        {
            throw new NotImplementedException();
        }

        public override void Send(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
