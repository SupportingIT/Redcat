using System;
using System.Collections.Generic;

namespace Redcat.Core
{
    public interface IKernel
    {
        ICollection<IServiceProvider> Providers { get; }
        T GetService<T>() where T : class;
        IEnumerable<T> GetServices<T>() where T : class;
        void AddEventHandler<T>(string eventId, Action<T> handler) where T : EventArgs;
        void RemoveEventHandler<T>(string eventId, Action<T> handler) where T : EventArgs;
        void RiseEvent<T>(string eventId, T args) where T : EventArgs;
    }
}
