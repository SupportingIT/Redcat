using System;
using System.Collections.Generic;

namespace Redcat.Core
{
    public class ServiceProvider : IServiceContainer, IServiceProvider
    {
        private IDictionary<Type, object> serviceInstances;
        private IDictionary<Type, Func<object>> serviceFactories;

        public ServiceProvider()
        {
            serviceInstances = new Dictionary<Type, object>();
            serviceFactories = new Dictionary<Type, Func<object>>();
        }

        public void Add<T>(T service)
        {
            serviceInstances[typeof(T)] = service;
        }

        public void AddFactory<T>(Func<T> factory)
        {
            serviceFactories[typeof(T)] = () => factory();
        }

        public object GetService(Type serviceType)
        {
            if (serviceInstances.ContainsKey(serviceType)) return serviceInstances[serviceType];
            if (serviceFactories.ContainsKey(serviceType)) return serviceFactories[serviceType]();
            return null;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public IEnumerable<T> GetServices<T>()
        {
            throw new NotImplementedException();
        }
    }
}
