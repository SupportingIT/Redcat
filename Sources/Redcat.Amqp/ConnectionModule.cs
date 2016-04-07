using Redcat.Amqp.Performatives;
using System;

namespace Redcat.Amqp
{
    public class ConnectionModule
    {
        private Action<AmqpFrame> sendAction;

        public ConnectionModule(Action<AmqpFrame> sendAction)
        {
            this.sendAction = sendAction;
        }

        public void OpenConnection()
        {
            Open open = new Open { ContainerId = "my-container" };
            sendAction(new AmqpFrame(open));
        }

        public void CloseConnection()
        { }
    }
}
