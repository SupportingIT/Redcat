using System;
using System.Collections.Generic;
using System.Linq;

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
            if (serviceInstances.ContainsKey(typeof(T))) AddToExistedValue<T>(service);
            else serviceInstances[typeof(T)] = service;
        }

        private void AddToExistedValue<T>(T service)
        {
            T value = (T)serviceInstances[typeof(T)];
            if (value is ICollection<T>) ((ICollection<T>)value).Add(service);
            else serviceInstances[typeof(T)] = new List<T> { value, service };            
        }

        public void AddFactory<T>(Func<T> factory)
        {            
            serviceFactories[typeof(T)] = () => factory();
        }

        public object GetService(Type serviceType)
        {
            if (serviceInstances.ContainsKey(serviceType)) return serviceInstances[serviceType];
            if (serviceFactories.ContainsKey(serviceType)) return serviceFactories[serviceType].Invoke();
            return null;
        }

        public T GetService<T>()
        {
            if (!serviceInstances.ContainsKey(typeof(T))) return default(T);
            object value = serviceInstances[typeof(T)];
            if (value is ICollection<T>) return ((ICollection<T>)value).First();            
            return (T)value;
        }

        public IEnumerable<T> GetServices<T>()
        {
            if (!serviceInstances.ContainsKey(typeof(T))) return Enumerable.Empty<T>();
            object value = serviceInstances[typeof(T)];
            if (value is IEnumerable<T>) return (IEnumerable<T>)value;
            return new T[1] { (T)value };
        }
    }
}
