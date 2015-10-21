using Redcat.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Redcat.Core
{
    public class CommandProcessor : IRunable, IDisposable
    {
        private IDictionary<string, Action<IServiceContainer>> extensions;
        private IServiceContainer serviceContainer;
        private bool initialized = false;

        public CommandProcessor()
        {
            extensions = new Dictionary<string, Action<IServiceContainer>>();
        }

        protected bool IsRunning
        {
            get { return initialized; }
        }

        public void Execute<T>(T command)
        {
            //Contract.Requires<ArgumentNullException>(command != null);
            foreach (var handler in GetHandlersForCommand<T>()) handler.Handle(command);
        }

        private IEnumerable<ICommandHandler<T>> GetHandlersForCommand<T>()
        {
            throw new NotImplementedException();
        }

        protected T GetService<T>()
        {
            return serviceContainer.GetService<T>();
        } 

        protected IEnumerable<T> GetServices<T>()
        {
            return serviceContainer.GetServices<T>();
        }

        public void AddCommandHandler<T>(ICommandHandler<T> handler)
        {
            //Contract.Requires<ArgumentNullException>(handler != null);            
        }

        public void AddExtension(string name, Action<IServiceContainer> extension)
        {
            //Contract.Requires<ArgumentNullException>(name != null);
            //Contract.Requires<ArgumentNullException>(extension != null);
            extensions.Add(name, extension);
        }

        public void Run()
        {
            if (initialized) return;
            serviceContainer = CreateServiceContainer();
            OnBeforeInit();
            OnInit();
            OnAfterInit();
            initialized = true;
        }

        protected virtual IServiceContainer CreateServiceContainer()
        {
            return new ServiceProvider();
        }

        protected virtual void OnBeforeInit()
        { }

        protected virtual void OnInit()
        {
            foreach (var extension in extensions.Values) extension(serviceContainer);
        }

        protected virtual void OnAfterInit()
        { }

        public void Dispose()
        { }
    }
}
