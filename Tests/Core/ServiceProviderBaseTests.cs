using NUnit.Framework;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class ServiceProviderBaseTests
    {
        [Test]
        public void GetService_Returns_Same_Instance_Wich_Was_Registered_By_AddServiceInstance()
        {
            ServiceProviderImpl provider = new ServiceProviderImpl();
            string service = "Some-Service";
            provider.AddServiceInstance(service);

            object actualService = provider.GetService(typeof(string));

            Assert.That(ReferenceEquals(actualService, service), Is.True);
        }

        [Test]
        public void GetService_Calls_Factory_Function_Wich_Was_Registered_By_AddServiceFactory()
        {
            ServiceProviderImpl provider = new ServiceProviderImpl();
            bool factoryWasCalled = false;
            int expectedResut = 8;
            provider.AddServiceFactory(() => {
                factoryWasCalled = true;
                return expectedResut;
            });

            int actualResult = (int)provider.GetService(typeof(int));

            Assert.That(factoryWasCalled, Is.True);
            Assert.That(actualResult, Is.EqualTo(expectedResut));
        }

        [Test]
        public void GetService_Returns_Null_If_No_Services_Was_Registered()
        {
            ServiceProviderImpl provider = new ServiceProviderImpl();

            object actulResult = provider.GetService(typeof(string));

            Assert.That(actulResult, Is.Null);
        }
    }

    internal class ServiceProviderImpl : ServiceProviderBase
    {
        internal new void AddServiceInstance<T>(T instance)
        {
            base.AddServiceInstance(instance);
        }

        internal new void AddServiceFactory<T>(Func<T> factory)
        {
            base.AddServiceFactory(factory);
        }
    }
}
