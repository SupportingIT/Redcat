using Prism.Modularity;
using System.Collections.Generic;
using Redcat.Core.Communication;
using Redcat.Xmpp.Communication;

namespace Redcat.Communicator
{
    public class XmppModule : IModule
    {
        private ICollection<IChannelFactory> factories;
        private IChannelFactory<IStreamChannel> streamChannelFactory;

        public XmppModule(ICollection<IChannelFactory> factories, IChannelFactory<IStreamChannel> streamChannelFactory)
        {
            this.factories = factories;
            this.streamChannelFactory = streamChannelFactory;
        }

        public void Initialize()
        {
            factories.Add(new XmppChannelFactory(streamChannelFactory));
        }
    }
}
