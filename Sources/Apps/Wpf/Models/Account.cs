using Redcat.Core;

namespace Redcat.Communicator.Models
{
    public class Account
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Protocol { get; set; }

        public ConnectionSettings ConnectionSettings { get; set; }
    }
}
