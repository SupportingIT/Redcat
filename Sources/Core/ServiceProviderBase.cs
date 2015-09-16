using System;
using System.Collections.Generic;

namespace Redcat.Core
{
    public abstract class ServiceProviderBase : IServiceProvider
    {
        private IDictionary<Type, object> serviceInstances;
        private IDictionary<Type, Func<object>> serviceFactories;

        protected ServiceProviderBase()
        {
            serviceInstances = new Dictionary<Type, object>();
            serviceFactories = new Dictionary<Type, Func<object>>();
        }

        protected void AddServiceInstance<T>(T service)
        {
            serviceInstances[typeof(T)] = service;
        }

        protected void AddServiceFactory<T>(Func<T> factory)
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
    }
}
