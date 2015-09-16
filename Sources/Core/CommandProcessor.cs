using Redcat.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Core
{
    public class CommandProcessor
    {
        private Kernel kernel = new Kernel();
        private ICollection<IKernelExtension> extensions = new List<IKernelExtension>();

        public void AddExtension(IKernelExtension extension)
        {
            extensions.Add(extension);
        }

        protected T Service<T>() where T : class
        {
            return kernel.GetService<T>();
        }

        protected IEnumerable<T> Services<T>() where T : class
        {
            return kernel.GetServices<T>();
        }

        protected void ForEachService<T>(Action<T> action, Action noServicesAction = null) where T : class
        {
            var services = Services<T>();
            if (noServicesAction != null && services.Count() == 0) noServicesAction();
            foreach (var service in Services<T>()) action(service);
        }

        public void Execute<T>(T command)
        {
            ForEachService<ICommandHandler<T>>(h => h.Handle(command), () => { throw new InvalidOperationException(); });
        }

        public void Run()
        {
            foreach (var extension in extensions) extension.Attach(kernel);
        }
    }
}
