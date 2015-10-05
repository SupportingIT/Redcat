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
            XmppStreamReader reader = new XmppStreamReader(stream);
            
            StreamHeader header = StreamHeader.CreateClientHeader(Settings.Domain);
            writer.Write(header);

            var responseHeader = reader.Read();
            var features = reader.Read().Childs;
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
