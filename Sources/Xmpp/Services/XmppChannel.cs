using System;
using Redcat.Core;
using Redcat.Core.Net;

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
            throw new NotImplementedException();
        }

        private void InitializeStream(IXmppStream stream)
        {
            var initializer = CreateStreamInitializer(Settings);
            initializer.Start(stream);
        }

        protected virtual IStreamInitializer CreateStreamInitializer(ConnectionSettings Settings)
        {
            throw new NotImplementedException();
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
