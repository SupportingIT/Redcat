using System;
using System.Collections.Generic;

namespace Redcat.Core.Services
{
    public class MessageDispatcher : IMessageDispatcher
    {        
        public ICollection<Action<Message>> IncomingMessageHandlers { get; } = new List<Action<Message>>();

        public ICollection<Action<Message>> OutgoingMessageHandlers { get; } = new List<Action<Message>>();

        public void DispatchIncoming(Message message)
        {
            foreach (var handler in IncomingMessageHandlers)
            {
                handler.Invoke(message);
            }
        }

        public void DispatchOutgoing(Message message)
        {
            foreach(var handler in OutgoingMessageHandlers)
            {
                handler.Invoke(message);
            }
        }
    }
}
