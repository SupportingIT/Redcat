using Microsoft.Extensions.DependencyInjection;
using Redcat.Core.Communication;
using Redcat.Core.Service;
using Redcat.Core.Service.DI;
using System;
using System.Collections.Generic;

namespace Redcat.Core
{
    public class Communicator : IDisposable
    {
        private IDictionary<string, Action<IServiceCollection>> extensions;
        private IServiceProvider serviceProvider;
        private bool initialized = false;

        private Lazy<IChannelManager> channelManager;
        private Lazy<IMessageDispatcher> messageDispatcher;

        public Communicator()
        {
            extensions = new Dictionary<string, Action<IServiceCollection>>();
            channelManager = new Lazy<IChannelManager>(GetService<IChannelManager>);
            messageDispatcher = new Lazy<IMessageDispatcher>(GetService<IMessageDispatcher>);
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

        protected IChannelManager ChannelManager
        {
            get { return channelManager.Value; }
        }

        protected IChannel DefaultChannel
        {
            get { return ChannelManager.DefaultChannel; }
        }

        protected IMessageDispatcher MessageDispatcher
        {
            get { return messageDispatcher.Value; }
        }

        public void AddExtension(string name, Action<IServiceCollection> extension)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (extension == null) throw new ArgumentNullException(nameof(extension));
            extensions.Add(name, extension);
        }

        public void Start()
        {
            if (initialized) return;
            OnBeforeInit();
            OnInit();
            OnAfterInit();
            initialized = true;
        }

        public void Connect(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (!IsRunning) throw new InvalidOperationException("Start method must be called before opening any channels");
            ChannelManager.OpenChannel(settings);
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Send<T>(T message)
        {
            MessageDispatcher.Dispatch(message);
        }

        public void Send(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (!IsRunning) throw new InvalidOperationException("Run method must be called before sending any messages");
            MessageDispatcher.Dispatch(message);
        }

        protected virtual void OnBeforeInit()
        {            
            AddExtension("Redcat.Communicator", CommunicatorExtension);
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

        private void CommunicatorExtension(IServiceCollection collection)
        {
            IChannelManager channelManager = new ChannelManager(GetServices<IChannelFactory>);            
            collection.TryAddSingleton(channelManager);
            collection.TryAddSingleton<IMessageDispatcher, MessageDispatcher>();
        }
    }
}
