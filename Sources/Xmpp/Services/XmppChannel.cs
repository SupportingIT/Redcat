using System;
using Redcat.Core;
using Redcat.Core.Communication;

namespace Redcat.Xmpp.Services
{
    public class XmppChannel : ChannelBase
    {
        private IStreamInitializer streamInitializer;
        private IXmppStream stream;

        public XmppChannel(IStreamInitializer streamInitializer, IStreamChannel transportChannel, ConnectionSettings settings) : base(settings)
        {
            if (streamInitializer == null) throw new ArgumentNullException(nameof(streamInitializer));
            this.streamInitializer = streamInitializer;
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            stream = CreateXmppStream();
            streamInitializer.Init(stream);
        }

        protected virtual IXmppStream CreateXmppStream()
        {
            throw new NotImplementedException();
        }

        internal void SetTlsStream()
        {
            throw new NotImplementedException();
        }
    }
}
