using System;
using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;
using Redcat.Xmpp.Parsing;
using Redcat.Core.Serializaton;

namespace Redcat.Xmpp.Channels
{
    public class XmppChannel : ReactiveChannelBase<XmlElement>, IReactiveXmppChannel, IXmppStream
    {
        private const int DefaultBufferSize = 1024;

        private ReactiveChannelAdapter<XmlElement> channelAdapter;

        public XmppChannel(IReactiveStreamChannel streamChannel, ConnectionSettings settings) : base(streamChannel, settings)
        {
            channelAdapter = new ReactiveChannelAdapter<XmlElement>(this);
        }

        public XmlElement Read() => channelAdapter.Receive();

        public void Write(XmlElement element) => Send(element);

        protected override IReactiveDeserializer<XmlElement> CreateDeserializer()
        {
            IXmlParser parser = new XmppStreamParser();
            return new XmlElementDeserializer(parser, DefaultBufferSize);
        }

        protected override ISerializer<XmlElement> CreateSerializer()
        {
            throw new NotImplementedException();
        }
    }
}
