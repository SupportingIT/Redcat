using System;
using System.Collections.Generic;

namespace Redcat.Core.Channels
{
    public class CompositeChannelFactory : IChannelFactory
    {
        private ICollection<IChannelFactory> Factories { get; } = new List<IChannelFactory>();

        public IChannel CreateChannel(ConnectionSettings settings)
        {
            IChannelFactory factory = SelectFactory(Factories, settings);
            return factory.CreateChannel(settings);
        }

        protected virtual IChannelFactory SelectFactory(IEnumerable<IChannelFactory> factories, ConnectionSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
