using System;
using System.Threading.Tasks;

namespace Redcat.Core.Channels
{
    public abstract class AsyncChannelBase : ChannelBase, IAsyncChannel
    {
        protected AsyncChannelBase(ConnectionSettings settings) : base(settings)
        { }

        public async Task CloseAsync()
        {
            await Task.Run(() => Close());
        }

        public async Task OpenAsync()
        {
            await Task.Run(() => Open());
        }
    }
}
