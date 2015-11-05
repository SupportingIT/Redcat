using Microsoft.Extensions.DependencyInjection;
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

        protected T GetService<T>()
        {
            return serviceProvider.GetService<T>();
        }

        protected IEnumerable<T> GetServices<T>()
        {
            return serviceProvider.GetServices<T>();
        }

        public void Execute<T>(T command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var handler = GetHandlerForCommand<T>();
            if (handler == null) throw new InvalidOperationException();
            handler.Handle(command);
        }

        private ICommandHandler<T> GetHandlerForCommand<T>()
        {
            return serviceProvider.GetService<ICommandHandler<T>>();
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
            serviceProvider = CreateServiceProvider(services);
        }

        private IServiceCollection CreateServiceCollection()
        {
            return new ServiceCollection();
        }

        private IServiceProvider CreateServiceProvider(IServiceCollection collection)
        {
            return new ServiceProvider(collection);
        }

        protected virtual void OnAfterInit()
        { }

        public void Dispose()
        { }
    }
}
