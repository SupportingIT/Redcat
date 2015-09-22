using System;
using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Xmpp.Xml;

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
            
            writer.Write("<?xml version='1.0' ?>");
            writer.Write("<stream:stream to='redcat' xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams' version='1.0' />");

            XmppStreamReader reader = XmppStreamReader.CreateReader(stream);
            StreamHeader header = reader.Read() as StreamHeader;

            Element features = reader.Read();
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
