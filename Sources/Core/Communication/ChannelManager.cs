using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Core.Communication
{
    public class ChannelManager : IChannelManager
    {
        private ICollection<IChannel> activeChannels;
        private IChannel defaultChannel;
        private IEnumerable<IChannelFactory> channelFactories;

        public ChannelManager(IEnumerable<IChannelFactory> channelFactories)
        {
            activeChannels = new List<IChannel>();
            defaultChannel = null;
            this.channelFactories = channelFactories;
        }

        public IEnumerable<IChannel> ActiveChannels
        {
            get { return activeChannels; }
        }

        public IChannel DefaultChannel
        {
            get { return defaultChannel; }
        }

        public IChannel OpenChannel(ConnectionSettings settings)
        {            
            IChannelFactory factory = SelectFactory(channelFactories, settings);

            if (factory == null) throw new InvalidOperationException("Unable to find channel factory");

            IChannel channel = factory.CreateChannel(settings);
            RegisterChannel(channel);
            UpdateDefaultChannel();
            return channel;
        }

        private void RegisterChannel(IChannel channel)
        {
            channel.StateChanged += OnChannelStateChanged;
            channel.Open();
            activeChannels.Add(channel);
        }

        private void OnChannelStateChanged(object sender, StateChangedEventArgs args)
        {
            if (args.NewState != ChannelState.Close) return;
            var channel = (IChannel)sender;
            activeChannels.Remove(channel);
            UpdateDefaultChannel();
        }

        private void UpdateDefaultChannel()
        {
            defaultChannel = SelectDefaultChannel(activeChannels);
        }

        protected virtual IChannel SelectDefaultChannel(IEnumerable<IChannel> activeChannels)
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
