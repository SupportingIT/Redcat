using Redcat.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Core
{
    public class CommandProcessor : IRunable, IDisposable
    {
        private IKernel kernel;
        private ICollection<IKernelExtension> extensions = new List<IKernelExtension>();
        private bool initialized = false;

        protected IKernel Kernel
        {
            get { return kernel; }
        }

        protected bool IsRunning
        {
            get { return initialized; }
        }

        public void AddExtension(IKernelExtension extension)
        {
            if (extension == null) throw new ArgumentNullException("extension");
            extensions.Add(extension);
        }

        public void AddExtensions(IEnumerable<IKernelExtension> extensions)
        {
            foreach (var extension in extensions) AddExtension(extension);
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
            foreach (var service in services) action(service);
        }

        public void Execute<T>(T command)
        {
            ForEachService<ICommandHandler<T>>(h => h.Handle(command), () => { throw new InvalidOperationException(); });
        }

        public void Run()
        {
            if (initialized) return;
            kernel = CreateKernel();
            OnBeforeInit();
            OnInit();
            OnAfterInit();
            initialized = true;
        }

        protected virtual void OnBeforeInit()
        { }

        protected virtual void OnInit()
        {            
            foreach (var extension in extensions) extension.Attach(kernel);
            if (kernel is IRunable) ((IRunable)kernel).Run();
        }

        protected virtual void OnAfterInit()
        { }

        protected virtual IKernel CreateKernel()
        {
            return new Kernel();
        }

        public void Dispose()
        {
            foreach (var extension in extensions) extension.Detach(kernel);
            if (kernel is IDisposable) ((IDisposable)kernel).Dispose();
        }
    }
}
