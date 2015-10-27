using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Core.Services
{
    public class EventHub : IEventHub
    {
        private IDictionary<string, ICollection<object>> handlers = new Dictionary<string, ICollection<object>>();

        public void AddEventHandler<T>(string eventId, Action<T> handler) where T : EventArgs
        {
            if (!handlers.ContainsKey(eventId)) handlers[eventId] = CreateCollection();
            handlers[eventId].Add(handler);
        }

        private ICollection<object> CreateCollection()
        {
            return new List<object>();
        }

        public void RemoveEventHandler<T>(string eventId, Action<T> handler) where T : EventArgs
        {
            if (!handlers.ContainsKey(eventId)) return;
            handlers[eventId].Remove(handler);
            if (handlers[eventId].Count == 0) handlers.Remove(eventId);
        }

        public void RiseEvent<T>(string eventId, T args) where T : EventArgs
        {
            foreach (var handler in Get<T>(eventId)) handler(args);
        }        

        private IEnumerable<Action<T>> Get<T>(string eventId)
        {
            if (!handlers.ContainsKey(eventId)) return Enumerable.Empty<Action<T>>();
            return handlers[eventId].OfType<Action<T>>();
        }
    }
}
