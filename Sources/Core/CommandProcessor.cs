using Redcat.Core.Service;
using Redcat.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Redcat.Core
{
    public class CommandProcessor : IRunable, IDisposable
    {
        private IDictionary<string, Action<IServiceCollection>> extensions;
        private IServiceProvider serviceProvider;
        private bool initialized = false;

        public CommandProcessor()
        {
            extensions = new Dictionary<string, Action<IServiceCollection>>();
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
            throw new NotImplementedException();
        } 

        protected IEnumerable<T> GetServices<T>()
        {
            throw new NotImplementedException();
        }

        public void AddCommandHandler<T>(ICommandHandler<T> handler)
        {
            //Contract.Requires<ArgumentNullException>(handler != null);            
        }

        public void AddExtension(string name, Action<IServiceCollection> extension)
        {
            //Contract.Requires<ArgumentNullException>(name != null);
            //Contract.Requires<ArgumentNullException>(extension != null);
            extensions.Add(name, extension);
        }

        public void Run()
        {
            if (initialized) return;            
            OnBeforeInit();
            OnInit();
            OnAfterInit();
            initialized = true;
        }

        protected virtual void OnBeforeInit()
        {
        }

        private IServiceCollection CreateServiceCollection()
        {
            throw new NotImplementedException();
        }

        private IServiceProvider CreateServiceProvider(IServiceCollection collection)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnInit()
        {
            //foreach (var extension in extensions.Values) extension(serviceContainer);
        }

        protected virtual void OnAfterInit()
        { }

        public void Dispose()
        { }
    }
}
