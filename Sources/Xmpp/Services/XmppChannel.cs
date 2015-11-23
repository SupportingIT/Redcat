using System;
using Redcat.Core;
using Redcat.Core.Communication;

namespace Redcat.Xmpp.Services
{
    public class XmppChannel : ChannelBase
    {
        private IStreamInitializer streamInitializer;
        private IStreamChannel streamChannel;
        private IXmppStream xmppStream;
        private StreamProxy streamProxy;

        public XmppChannel(IStreamInitializer streamInitializer, IStreamChannel streamChannel, ConnectionSettings settings) : base(settings)
        {
            if (streamInitializer == null) throw new ArgumentNullException(nameof(streamInitializer));
            if (streamChannel == null) throw new ArgumentNullException(nameof(streamChannel));
            this.streamInitializer = streamInitializer;
            this.streamChannel = streamChannel;            
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            streamChannel.Open();            
            xmppStream = CreateXmppStream();
            streamInitializer.Init(xmppStream);
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            streamChannel.Close();
        }

        protected virtual IXmppStream CreateXmppStream()
        {
            streamProxy = new StreamProxy(streamChannel.GetStream());
            return new XmppStream(streamProxy);
        }

        internal void SetTlsContext()
        {
            if (!(streamChannel is ISecureStreamChannel)) throw new InvalidOperationException();
            streamProxy.OriginStream = ((ISecureStreamChannel)streamChannel).GetSecureStream();
        }
    }
}
