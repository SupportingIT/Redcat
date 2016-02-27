using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Channels;
using FakeItEasy;
using Redcat.Core.Channels;
using System.IO;
using System;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Channels
{
    [TestFixture]
    public class XmppChannelTests
    {
        [Test]
        public void Open_Initializes_Stream()
        {
            ConnectionSettings settings = new ConnectionSettings { Domain = "redcat" };
            IXmppStream xmppStream = A.Fake<IXmppStream>();
            IStreamChannel streamChannel = CreateStreamChannel();
            var initializer = A.Fake<Action<IXmppStream>>();

            XmppChannel channel = new XmppChannel(streamChannel, settings);
            channel.Initializer = initializer;

            channel.Open();

            A.CallTo(() => initializer.Invoke(A<IXmppStream>._)).MustHaveHappened();
        }

        private IStreamChannel CreateStreamChannel()
        {
            IStreamChannel streamChannel = A.Fake<IStreamChannel>();
            Stream stream = A.Fake<Stream>();

            A.CallTo(() => stream.CanRead).Returns(true);
            A.CallTo(() => stream.CanWrite).Returns(true);
            A.CallTo(() => streamChannel.GetStream()).Returns(stream);

            return streamChannel;
        }
    }
}
