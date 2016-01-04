using System;
using System.Threading.Tasks;
using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Channels
{
    public class XmppChannel : ChannelBase, IInputChannel<Stanza>, IAsyncInputChannel<Stanza>, IOutputChannel<Stanza>, IAsyncOutputChannel<Stanza>
    {
        private IStreamInitializer streamInitializer;
        private IStreamChannel streamChannel;
        private XmppStream xmppStream;
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

        protected virtual XmppStream CreateXmppStream()
        {
            streamProxy = new StreamProxy(streamChannel.GetStream());
            return new XmppStream(streamProxy);
        }

        internal void SetTlsContext()
        {
            if (!(streamChannel is ISecureStreamChannel)) throw new InvalidOperationException();
            streamProxy.OriginStream = ((ISecureStreamChannel)streamChannel).GetSecureStream();
        }

        public Stanza Receive()
        {
            return xmppStream.Read() as Stanza;
        }

        public async Task<Stanza> ReceiveAsync()
        {
            return await xmppStream.ReadAsync() as Stanza;
        }

        public void Send(Stanza stanza)
        {
            xmppStream.Write(stanza);
        }

        public async Task SendAsync(Stanza stanza)
        {
            await xmppStream.WriteAsync(stanza);
        }
    }
}
