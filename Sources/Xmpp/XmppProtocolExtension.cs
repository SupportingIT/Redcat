using System;
using Redcat.Core;
using Redcat.Core.Net;

namespace Redcat.Xmpp
{
    public class XmppProtocolExtension : IKernelExtension
    {
        private Func<ISocket> socketFactory;

        public XmppProtocolExtension(Func<ISocket> socketFactory)
        {
            this.socketFactory = socketFactory;
        }

        public void Attach(IKernel kernel)
        {
            kernel.Providers.Add(new XmppServiceProvider(socketFactory));
        }

        public void Detach(IKernel kernel)
        {
            throw new NotImplementedException();
        }
    }
}
