using FakeItEasy;
using NUnit.Framework;
using System;
using System.Linq;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class KernelTests
    {
        [Test]
        public void GetService_Returns_Service_Provided_By_ServiceProvider()
        {
            Kernel kernel = new Kernel();
            string service = "my-service";
            IServiceProvider provider = A.Fake<IServiceProvider>();
            A.CallTo(() => provider.GetService(typeof(string))).Returns(service);
            kernel.Providers.Add(provider);

            string actualService = kernel.GetService<string>();

            Assert.That(ReferenceEquals(actualService, service), Is.True);
        }

        [Test]
        public void GetService_Returns_Null_If_No_Providers_For_Service()
        {
            Kernel kernel = new Kernel();
            IServiceProvider provider = A.Fake<IServiceProvider>();
            kernel.Providers.Add(provider);

            Uri actualService = kernel.GetService<Uri>();

            Assert.That(actualService, Is.Null);
        }

        [Test]
        public void GetServices_Returns_Services_Provided_By_All_ServiceProviders()
        {
            var providers = A.CollectionOfFake<IServiceProvider>(3);
            A.CallTo(() => providers[0].GetService(typeof(string))).Returns("service0");
            A.CallTo(() => providers[1].GetService(typeof(string))).Returns(null);
            A.CallTo(() => providers[2].GetService(typeof(string))).Returns("service2");
            Kernel kernel = new Kernel();
            foreach (var provider in providers) kernel.Providers.Add(provider);

            var actualServices = kernel.GetServices<string>().ToArray();

            Assert.That(actualServices.Length, Is.EqualTo(2));
            Assert.That(actualServices.All(s => s != null));
        }

        [Test]
        public void RiseEvent_Calls_Added_Event_Handler()
        {
            Kernel kernel = new Kernel();
            bool handlerWasCalled = false;
            kernel.AddEventHandler<EventArgs>("my-event", args => handlerWasCalled = true);

            kernel.RiseEvent("my-event", EventArgs.Empty);

            Assert.That(handlerWasCalled, Is.True);
        }

        [Test]
        public void RiseEvent_Calls_Event_Handler_With_Correct_EventArgs_Type()
        {
            Kernel kernel = new Kernel();
            bool handlerWasCalled = false;
            kernel.AddEventHandler<EventArgs>("my-event", args => handlerWasCalled = true);
            kernel.AddEventHandler<AssemblyLoadEventArgs>("my-event", args => handlerWasCalled = false);

            kernel.RiseEvent("my-event", EventArgs.Empty);

            Assert.That(handlerWasCalled, Is.True);
        }

        [Test]
        public void RiseEvent_Dont_Calls_Removed_Event_Handler()
        {
            Kernel kernel = new Kernel();
            bool handlerWasCalled = false;
            Action<EventArgs> handler = args => handlerWasCalled = true;            

            kernel.AddEventHandler("my-event", handler);
            kernel.RemoveEventHandler("my-event", handler);
            kernel.RiseEvent("my-event", EventArgs.Empty);

            Assert.That(handlerWasCalled, Is.False);
        }
    }
}
