using System.Collections.Generic;

namespace Redcat.Core
{
    public interface IServiceContainer
    {
        void Add<T>(T service);
        T GetService<T>();
        IEnumerable<T> GetServices<T>();
    }
}
