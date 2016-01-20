using System;

namespace Redcat.Xmpp.Negotiators
{
    public interface ISaslCredentials : IDisposable
    {
        string Username { get; }
        string Password { get; }
    }
}
