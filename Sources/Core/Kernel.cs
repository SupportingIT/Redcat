using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Core
{
    public class Kernel : IKernel
    {
        private List<IServiceProvider> providers = new List<IServiceProvider>();
        
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
    }    
}
