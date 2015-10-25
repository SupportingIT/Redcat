using System;
using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Xmpp.Negotiators;

namespace Redcat.Xmpp.Services
{
    public class XmppChannel : NetworkChannel
    {
        private IXmppStream stream;

        public XmppChannel(ISocket socket, ConnectionSettings settings) : base(socket, settings)
        { }

        protected override void OnOpening()
        {
            base.OnOpening();
            stream = OpenXmppStream();
            InitializeStream(stream);
        }

        protected virtual IXmppStream OpenXmppStream()
        {
            SocketStream stream = new SocketStream(Socket);
            return new XmppStream(stream);
        }

        private void InitializeStream(IXmppStream stream)
        {
            var initializer = CreateStreamInitializer(Settings);
            initializer.Start(stream);
        }

        protected virtual IStreamInitializer CreateStreamInitializer(ConnectionSettings settings)
        {            
            var initializer = new StreamInitializer(settings);
            TlsNegotiator tls = new TlsNegotiator(null);
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
