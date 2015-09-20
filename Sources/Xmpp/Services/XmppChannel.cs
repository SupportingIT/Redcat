using System;
using Redcat.Core;
using Redcat.Core.Net;

namespace Redcat.Xmpp.Services
{
    public class XmppChannel : NetworkChannel
    {
        public XmppChannel(ISocket socket, ConnectionSettings settings) : base(socket, settings)
        { }

        protected override void OnOpening()
        {
            base.OnOpening();
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
