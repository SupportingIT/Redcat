using System;
using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Channels
{
    public class XmppChannel : BufferChannel<XmlElement>, IDuplexChannel<XmlElement>
    {
        private const int DefaultBufferSize = 1024;
        private IStreamChannel streamChannel;        
        private StreamProxy streamProxy;
        private IDisposable subscription;

        private XmppStreamWriter writer;

        public XmppChannel(IStreamChannel streamChannel, ConnectionSettings settings) : base(DefaultBufferSize, settings)
        {
            if (streamChannel == null) throw new ArgumentNullException(nameof(streamChannel));
            this.streamChannel = streamChannel;
            if (streamChannel is IObservable<ArraySegment<byte>>)
            {
                subscription = ((IObservable<ArraySegment<byte>>)streamChannel).Subscribe(this);
            }
        }

        public Action<IDuplexChannel<XmlElement>> StreamInitializer { get; set; }

        protected override void OnOpening()
        {
            base.OnOpening();
            streamChannel.Open();
            streamProxy = new StreamProxy(streamChannel.GetStream());
            writer = new XmppStreamWriter(streamProxy);
            StreamInitializer?.Invoke(this);
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            streamChannel.Close();
        }

        internal void SetTlsContext()
        {
            if (!(streamChannel is ISecureStreamChannel)) throw new InvalidOperationException();
            streamProxy.OriginStream = ((ISecureStreamChannel)streamChannel).GetSecureStream();
        }

        public void Send(XmlElement message)
        {
            writer.Write(message);
        }
    }
}
