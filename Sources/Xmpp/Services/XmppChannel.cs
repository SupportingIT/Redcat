using System;
using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Xmpp.Negotiators;

namespace Redcat.Xmpp.Services
{
    public class XmppChannel : NetworkChannel
    {
        private IXmppStream stream;

        public XmppChannel(INetworkStreamFactory factory, ConnectionSettings settings) : base(factory, settings)
        { }

        protected override void OnOpening()
        {
            base.OnOpening();
            stream = OpenXmppStream();
            InitializeStream(stream);
        }

        protected virtual IXmppStream OpenXmppStream()
        {
            return new XmppStream(Stream);
        }

        private void InitializeStream(IXmppStream stream)
        {
            var initializer = CreateStreamInitializer(Settings);
            initializer.Start(stream);
        }

        protected virtual IStreamInitializer CreateStreamInitializer(ConnectionSettings settings)
        {            
            var initializer = new StreamInitializer(settings);
            TlsNegotiator tls = new TlsNegotiator(SetSecuredStream);
            initializer.Negotiators.Add(tls);
            return initializer;
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
