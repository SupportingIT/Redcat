using Redcat.Core.Communication;
using Redcat.Core.Service;
using Redcat.Core.Service.DI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Core
{
    public class CommandProcessor : IRunable, IDisposable
    {
        private IDictionary<string, Action<IServiceCollection>> extensions;
        private IServiceLocator serviceLocator;
        private bool initialized = false;

        public CommandProcessor()
        {
            extensions = new Dictionary<string, Action<IServiceCollection>>();
        }

        protected bool IsRunning
        {
            get { return initialized; }
        }

        protected T GetService<T>()
        {
            return default(T);// serviceProvider.GetService<T>();
        }

        protected IEnumerable<T> GetServices<T>()
        {
            return null;// serviceProvider.GetServices<T>();
        }

        public void Execute<T>(T command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var handlers = GetHandlersForCommand<T>();
            if (handlers.Count() == 0) throw new InvalidOperationException();
            foreach (var handler in handlers) handler.Handle(command);
        }

        private IEnumerable<ICommandHandler<T>> GetHandlersForCommand<T>()
        {
            return null;// serviceProvider.GetServices<ICommandHandler<T>>();
        }

        public void AddExtension(string name, Action<IServiceCollection> extension)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (extension == null) throw new ArgumentNullException(nameof(extension));
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

        protected virtual void OnInit()
        {
            var services = CreateServiceCollection();            
            foreach (var extension in extensions.Values) extension(services);
            serviceLocator = CreateServiceLocator(services);
        }

        private IServiceCollection CreateServiceCollection()
        {
            return new ServiceCollection();
        }

        private IServiceLocator CreateServiceLocator(IServiceCollection collection)
        {
            return null;
        }

        protected virtual void OnAfterInit()
        { }

        public void Dispose()
        { }
    }
}
