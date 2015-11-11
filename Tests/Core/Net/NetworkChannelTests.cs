using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Net;
using System;
using System.IO;

namespace Redcat.Core.Tests.Net
{
    [TestFixture]
    public class NetworkChannelTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Throws_Exception_If_NetworkStreamFactory_Is_Null()
        {
            NetworkChannel channel = CreateChannel(null);
        }

        [Test]
        public void Open_Creates_New_Stream()
        {
            INetworkStreamFactory factory = A.Fake<INetworkStreamFactory>();
            ConnectionSettings settings = new ConnectionSettings();
            NetworkChannel channel = CreateChannel(factory, settings);

            channel.Open();

            A.CallTo(() => factory.CreateStream(settings)).MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Send_Throws_Exception_If_Buffer_Is_Null()
        {
            NetworkChannel channel = CreateChannel(A.Fake<INetworkStreamFactory>());

            channel.Send(null, 0, 1);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Send_Throws_Exception_If_Channel_Was_Not_Opened()
        {
            NetworkChannel channel = CreateChannel(A.Fake<INetworkStreamFactory>());

            channel.Send(new byte[] { 0x01, 0x02, 0x03 }, 0, 3);
        }

        [Test]
        public void Send_Uses_Created_Stream()
        {
            var factory = A.Fake<INetworkStreamFactory>();
            var stream = A.Fake<Stream>();
            A.CallTo(() => factory.CreateStream(A<ConnectionSettings>._)).Returns(stream);
            var channel = CreateChannel(factory);
            byte[] buffer = new byte[] { 1, 2, 3 };

            channel.Open();
            channel.Send(buffer, 0, buffer.Length);

            A.CallTo(() => stream.Write(buffer, 0, buffer.Length)).MustHaveHappened();
        }

        private NetworkChannel CreateChannel(INetworkStreamFactory factory, ConnectionSettings settings = null)
        {
            if (settings == null) settings = new ConnectionSettings();
            return A.Fake<NetworkChannel>(b =>
            {
                b.WithArgumentsForConstructor(new object[] { factory, settings });
                b.CallsBaseMethods();
            });
        }
    }
}
