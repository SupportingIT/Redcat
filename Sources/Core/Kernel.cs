using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Core
{
    public class Kernel : IKernel
    {
        private List<IServiceProvider> providers = new List<IServiceProvider>();
        private EventHandlerCollection handlers = new EventHandlerCollection();

        public ICollection<IServiceProvider> Providers
        {
            get { return providers; }
        }

        public T GetService<T>() where T : class
        {
            //T service = null;
            //providers.FirstOrDefault(p => (service = p.GetService(typeof(T)) as T) != null);
            return providers.Select(p => p.GetService(typeof(T)) as T).FirstOrDefault(s => s != null);                        
        }

        public IEnumerable<T> GetServices<T>() where T : class
        {
            return providers.Select(p => p.GetService(typeof(T)) as T).Where(s => s != null);
        }

        public void AddEventHandler<T>(string eventId, Action<T> handler) where T : EventArgs
        {
            handlers.Add(eventId, handler);
        }

        public void RemoveEventHandler<T>(string eventId, Action<T> handler) where T : EventArgs
        {
            handlers.Remove(eventId, handler);
        }

        public void RiseEvent<T>(string eventId, T args) where T : EventArgs
        {            
            foreach (var handler in handlers.Get<T>(eventId)) handler(args);
        }
    }

    internal class EventHandlerCollection
    {
        private IDictionary<string, ICollection<object>> handlers = new Dictionary<string, ICollection<object>>();

        public void Add<T>(string eventId, Action<T> handler)
        {
            if (!handlers.ContainsKey(eventId)) handlers[eventId] = CreateCollection();
            handlers[eventId].Add(handler);
        }

        private ICollection<object> CreateCollection()
        {
            return new List<object>();
        }

        public void Remove<T>(string eventId, Action<T> handler)
        {
            if (!handlers.ContainsKey(eventId)) return;
            handlers[eventId].Remove(handler);
            if (handlers[eventId].Count == 0) handlers.Remove(eventId);
        }

        public IEnumerable<Action<T>> Get<T>(string eventId)
        {
            if (!handlers.ContainsKey(eventId)) return Enumerable.Empty<Action<T>>();
            return handlers[eventId].OfType<Action<T>>();
        }
    }
}
