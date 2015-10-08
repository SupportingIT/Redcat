using System;
using System.Net;
using Redcat.Core;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp
{
    public class StreamInitializer : IStreamInitializer
    {
        private ConnectionSettings settings;

        public StreamInitializer(ConnectionSettings settings)
        {
            this.settings = settings;
        }

        public void Start(IXmppStream stream)
        {
            SendHeader(stream);

            var response = stream.Read();
            VerifyResponseHeader(response);
            response = stream.Read();
            VerifyStreamFeatures(response);
        }

        private void SendHeader(IXmppStream stream)
        {
            StreamHeader header = StreamHeader.CreateClientHeader(settings.Domain);
            stream.Write(header);
        }

        private void VerifyResponseHeader(XmlElement response)
        {
        }

        private void VerifyStreamFeatures(XmlElement response)
        {
        }
    }
}
