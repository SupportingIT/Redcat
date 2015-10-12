using System;
using System.Collections.Generic;

namespace Redcat.Core
{
    public interface IKernel
    {
        ICollection<IServiceProvider> Providers { get; }
        T GetService<T>() where T : class;
        IEnumerable<T> GetServices<T>() where T : class;        
    }
}
