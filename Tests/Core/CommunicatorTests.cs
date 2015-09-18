using NUnit.Framework;
using Redcat.Core.Services;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class CommunicatorTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Connect_Throws_Exception_If_ConnectionSetings_Is_Null()
        {
            Communicator communicator = new Communicator();
            communicator.Connect(null);
        }

        [Test]
        public void Connect_Creates_Channel_Using_Correct_Factory()
        {
            IKernelExtension extension = CreateExtension();
            Communicator communicator = new Communicator();
            communicator.AddExtension(extension);
            communicator.Run();
            ConnectionSettings settings = new ConnectionSettings();

            communicator.Connect(settings);

            Assert.Fail();
        }

        private IKernelExtension CreateExtension(params IChannelFactory[] factories)
        {
            throw new NotImplementedException();
        }
    }
}
