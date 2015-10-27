using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Core.Communication
{
    public class ChannelManager : IChannelManager
    {
        private ICollection<IMessageChannel> activeChannels;
        private IMessageChannel defaultChannel;
        private Func<IEnumerable<IChannelFactory>> GetChannelFactories;

        public ChannelManager(Func<IEnumerable<IChannelFactory>> factoryProvider)
        {
            activeChannels = new List<IMessageChannel>();
            defaultChannel = null;
            GetChannelFactories = factoryProvider;
        }

        public IEnumerable<IMessageChannel> ActiveChannels
        {
            get { return activeChannels; }
        }

        public IMessageChannel DefaultChannel
        {
            get { return defaultChannel; }
        }

        public IMessageChannel OpenChannel(ConnectionSettings settings)
        {
            var factories = GetChannelFactories();
            IChannelFactory factory = SelectFactory(factories, settings);

            if (factory == null) throw new InvalidOperationException("Unable to find channel factory");

            IMessageChannel channel = factory.CreateChannel(settings);
            RegisterChannel(channel);
            UpdateDefaultChannel();
            return channel;
        }

        private void RegisterChannel(IMessageChannel channel)
        {
            channel.StateChanged += OnChannelStateChanged;
            channel.Open();
            activeChannels.Add(channel);
        }

        private void OnChannelStateChanged(object sender, StateChangedEventArgs args)
        {
            if (args.NewState != ChannelState.Close) return;
            var channel = (IMessageChannel)sender;
            activeChannels.Remove(channel);
            UpdateDefaultChannel();
        }

        private void UpdateDefaultChannel()
        {
            defaultChannel = SelectDefaultChannel(activeChannels);
        }

        protected virtual IMessageChannel SelectDefaultChannel(IEnumerable<IMessageChannel> activeChannels)
        {
            return activeChannels.LastOrDefault();
        }

        protected virtual IChannelFactory SelectFactory(IEnumerable<IChannelFactory> factories, ConnectionSettings settings)
        {
            foreach (var factory in factories)
            {
                string id = GetFactoryId(factory);
                if (settings.ChannelType == id) return factory;
            }

            return null;
        }

        private string GetFactoryId(IChannelFactory factory)
        {
            string id = factory.GetType().Name;
            int index = id.LastIndexOf("ChannelFactory");
            return id.Substring(0, index).ToLower();
        }
    }
}
