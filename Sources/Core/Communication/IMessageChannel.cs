using System;

namespace Redcat.Core.Communication
{
    public interface IMessageChannel : IDuplexChannel<Message>
    {    }    
}
