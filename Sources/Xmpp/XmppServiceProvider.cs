using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Core.Services;
using Redcat.Xmpp.Services;
using System;

namespace Redcat.Xmpp
{
    public class XmppServiceProvider : ServiceProviderBase
    {
        public XmppServiceProvider(Func<ISocket> socketFactory)
        {
            AddServiceInstance<IChannelFactory>(new XmppChannelFactory(socketFactory));
        }
    }
}
