using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Services;
using FakeItEasy;
using Redcat.Core.Net;

namespace Redcat.Xmpp.Tests.Services
{
    [TestFixture]
    public class XmppChannelTests
    {
        [Test]
        public void Open_Initializes_Stream()
        {
            ConnectionSettings settings = new ConnectionSettings { Domain = "redcat" };
            IStreamInitializer initializer = A.Fake<IStreamInitializer>();
            IXmppStream stream = A.Fake<IXmppStream>();
            TestXmppChannel channel = new TestXmppChannel(A.Fake<INetworkStreamFactory>(), settings) { Initializer = initializer, Stream = stream };

            channel.Open();

            A.CallTo(() => initializer.Init(stream)).MustHaveHappened();
        }

        internal class TestXmppChannel : XmppChannel
        {
            private IXmppStream stream;
            
            public TestXmppChannel(INetworkStreamFactory factory, ConnectionSettings settings) : base(A.Fake<IStreamInitializer>(), factory, settings)
            {
            }

            public IStreamInitializer Initializer { get; set; }

            public IXmppStream Stream { get; set; }

            protected override IXmppStream CreateXmppStream()
            {
                return Stream ?? base.CreateXmppStream();
            }
        }
    }
}
