using System;

namespace Redcat.Core.Services
{
    public interface IEventHub
    {
        void AddEventHandler<T>(string eventId, Action<T> handler) where T : EventArgs;
        void RemoveEventHandler<T>(string eventId, Action<T> handler) where T : EventArgs;
        void RiseEvent<T>(string eventId, T args) where T : EventArgs;
    }
}
