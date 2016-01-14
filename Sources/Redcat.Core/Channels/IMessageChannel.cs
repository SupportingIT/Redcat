using System;

namespace Redcat.Core.Channels
{
    public interface IMessageChannel : IDuplexChannel<Message>
    {    }    
}
