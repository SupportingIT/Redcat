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
        { }

        public void CloseConnection()
        { }
    }
}
