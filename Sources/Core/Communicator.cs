using System;

namespace Redcat.Core
{
    public class Communicator : CommandProcessor
    {
        public void Connect(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Send(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
