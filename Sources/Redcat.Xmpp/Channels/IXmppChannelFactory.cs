using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Channels
{
    public interface IXmppChannel : IOutputChannel<XmlElement>
    { }

    public interface IXmppChannelFactory : IChannelFactory<IXmppChannel>
    { }
}
