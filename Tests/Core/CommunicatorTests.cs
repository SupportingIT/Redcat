using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Services;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class CommunicatorTests
    {
        [Test]
        public void Connect_Creates_Channel()
        {
            ConnectionSettings settings = new ConnectionSettings { ChannelType = "test" };
            Communicator communicator = new Communicator();
            IChannelFactory factory = A.Fake<TestChannelFactory>();
            communicator.AddExtension("test", c => { c.Add(factory); });
            //communicator.Run();

            communicator.Connect(settings);

            A.CallTo(() => factory.CreateChannel(settings)).MustHaveHappened();
        }
    }

    public class TestChannelFactory : IChannelFactory
    {
        public virtual IMessageChannel CreateChannel(ConnectionSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
