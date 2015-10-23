using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using StringSet = System.Collections.Generic.IEnumerable<string>;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class ServiceProviderTests
    {
        [Test]
        public void GetServiceT_Returns_Same_Instance_Wich_Was_Registered_By_Add()
        {
            ServiceProvider provider = new ServiceProvider();
            string service = "Some-Service";
            provider.Add(service);

            string actualService = provider.GetService<string>();

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

            object actualResult = provider.GetService(typeof(int));

            Assert.That(factoryWasCalled, Is.True);
            Assert.That(actualResult, Is.EqualTo(expectedResut));
        }

        [Test]
        public void GetServiceT_Returns_Null_If_No_Services_Was_Registered()
        {
            ServiceProvider provider = new ServiceProvider();

            string actulResult = provider.GetService<string>();

            Assert.That(actulResult, Is.Null);
        }

        [Test]
        public void GetServiceT_Returns_Instance()
        {
            ServiceProvider provider = new ServiceProvider();
            string[] service1 = new string[0];
            string service2 = "";
            provider.Add<StringSet>(service1);
            provider.Add<object>(service2);

            Assert.That(provider.GetService<StringSet>(), Is.EqualTo(service1));
            Assert.That(provider.GetService<object>(), Is.EqualTo(service2));
        }

        [Test]
        public void GetServiceT_Returns_Instance_If_Multiple_Services_With_Same_Type_Was_Added()
        {
            ServiceProvider provider = new ServiceProvider();
            int service1 = 1;
            string service2 = "service2";
            provider.Add<object>(service1);
            provider.Add<object>(service2);

            object actualService = provider.GetService<object>();

            Assert.That(actualService.Equals(service1) || actualService.Equals(service2), Is.True);
        }

        [Test]
        public void GetServicesT_Returns_Empty_Collection_If_No_Service_Registered()
        {
            ServiceProvider provider = new ServiceProvider();

            var services = provider.GetServices<string>();

            Assert.That(services, Is.Empty);
        }

        [Test]
        public void GetServicesT_Single_Service_Registered_Returns_Collection_With_Single_Service()
        {
            ServiceProvider provider = new ServiceProvider();
            Guid service = Guid.NewGuid();
            provider.Add(service);

            var services = provider.GetServices<Guid>();
                        
            Assert.That(services.Single(), Is.EqualTo(service));
        }

        [Test]
        public void GetServices_Returns_All_Services()
        {
            List<string> service1 = new List<string>();
            string[] service2 = new string[0];
            ServiceProvider provider = new ServiceProvider();
            provider.Add<StringSet>(service1);
            provider.Add<StringSet>(service2);

            var services = provider.GetServices<StringSet>();

            Assert.That(services, Is.EquivalentTo(new StringSet[] { service1, service2 }));
        }
    }
}
