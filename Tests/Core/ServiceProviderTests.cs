using NUnit.Framework;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class ServiceProviderTests
    {
        [Test]
        public void GetService_Returns_Same_Instance_Wich_Was_Registered_By_Add()
        {
            ServiceProvider provider = new ServiceProvider();
            string service = "Some-Service";
            provider.Add(service);

            object actualService = provider.GetService(typeof(string));

            Assert.That(ReferenceEquals(actualService, service), Is.True);
        }

        [Test]
        public void GetService_Calls_Factory_Function_Wich_Was_Registered_By_AddFactory()
        {
            ServiceProvider provider = new ServiceProvider();
            bool factoryWasCalled = false;
            int expectedResut = 8;
            provider.AddFactory(() => {
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
            ServiceProvider provider = new ServiceProvider();

            object actulResult = provider.GetService(typeof(string));

            Assert.That(actulResult, Is.Null);
        }
    }
}
