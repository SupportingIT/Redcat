using Redcat.Core;

namespace Redcat.Xmpp
{
    public interface IStreamInitializerFactory
    {
        IStreamInitializer CreateInitializer(ConnectionSettings settings);
    }
}
