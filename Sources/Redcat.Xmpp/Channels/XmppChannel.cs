using System;
using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;
using System.IO;

namespace Redcat.Xmpp.Channels
{
    public class XmppChannel : BufferChannel<XmlElement>, IDuplexChannel<XmlElement>
    {
        private const int DefaultBufferSize = 1024;
        private IStreamChannel streamChannel;        
        private IDisposable subscription;

        private XmppStreamWriter writer;
        private Stream stream;

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
            stream = streamChannel.GetStream();
            writer = new XmppStreamWriter(stream);
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
            ((ISecureStreamChannel)streamChannel).SetStreamSecurity();
        }

        public void Send(XmlElement message)
        {
            writer.Write(message);
        }

        protected override void OnBufferUpdated()
        {
            throw new NotImplementedException();
        }
    }
}
