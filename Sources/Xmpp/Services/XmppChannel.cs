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

            SocketStream stream = new SocketStream(Socket);
            XmppStreamWriter writer = new XmppStreamWriter(stream);
            XmppStreamReader reader = XmppStreamReader.CreateReader(stream);
            writer.Write("<?xml version='1.0' ?>");
            writer.Write("<stream:stream to='redcat' xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams' version='1.0'>");

            var response = reader.Read();            
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
