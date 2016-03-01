using System;
using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;
using Redcat.Xmpp.Parsing;

namespace Redcat.Xmpp.Channels
{
    public class XmppChannel : BufferChannel<XmlElement>, IXmppChannel, IXmppStream, IObserver<XmlElement>
    {
        private const int DefaultBufferSize = 1024;
        private IStreamChannel streamChannel;        
        private IDisposable subscription;
        private IXmlParser parser;

        private XmppStreamWriter writer;

        private NegotiationContext context;

        public XmppChannel(IStreamChannel streamChannel, ConnectionSettings settings) : base(DefaultBufferSize, settings)
        {
            if (streamChannel == null) throw new ArgumentNullException(nameof(streamChannel));
            parser = new XmppStreamParser();
            this.streamChannel = streamChannel;
            if (streamChannel is IObservable<ArraySegment<byte>>)
            {
                subscription = ((IObservable<ArraySegment<byte>>)streamChannel).Subscribe(this);
            }
        }

        public NegotiationContext Context => context;

        public Func<IXmppStream, NegotiationContext> Initializer { get; set; }

        protected override void OnOpening()
        {
            base.OnOpening();
            streamChannel.Open();
            writer = new XmppStreamWriter(streamChannel.GetStream());
            context = Initializer?.Invoke(this);
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            streamChannel.Close();
        }

        public void OnNext(XmlElement element) => Send(element);

        internal void SetTlsContext()
        {
            if (!(streamChannel is ISecureStreamChannel)) throw new InvalidOperationException();
            writer = new XmppStreamWriter(((ISecureStreamChannel)streamChannel).GetSecuredStream());
        }

        public XmlElement Read() => Receive();

        public void Write(XmlElement element) => Send(element);

        public void Send(XmlElement message)
        {
            writer.Write(message);
        }

        protected override void OnBufferUpdated()
        {
            for (int i = Buffer.Count - 1; i >= 0; i--)
            {
                if (Buffer[i] == '>')
                {
                    var elements = parser.Parse(Buffer.ToString(0, i + 1));
                    EnqueueMessages(elements);
                    Buffer.Discard(i + 1);
                    break;
                }
            }
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            if (subscription != null) subscription.Dispose();
            streamChannel.DisposeIfDisposable();
            writer.Dispose();
        }
    }
}
