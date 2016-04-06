using System;
using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;
using Redcat.Xmpp.Parsing;

namespace Redcat.Xmpp.Channels
{
    public class XmppChannel : ReactiveChannelBase<XmlElement>, IReactiveXmppChannel, IXmppStream
    {
        private const int DefaultBufferSize = 1024;

        public XmppChannel(IReactiveStreamChannel streamChannel, ConnectionSettings settings) : base(streamChannel, settings)
        { }

        public XmlElement Read()
        {
            throw new NotImplementedException();
        }

        public void Write(XmlElement element)
        {
            throw new NotImplementedException();
        }

        protected override IReactiveDeserializer<XmlElement> CreateDeserializer()
        {
            throw new NotImplementedException();
        }

        protected override ISerializer<XmlElement> CreateSerializer()
        {
            throw new NotImplementedException();
        }
    }
}
