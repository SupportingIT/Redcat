using Redcat.Core;

namespace Redcat.App.Services
{
    public interface IConnectionSettingsRepository
    {
        ConnectionSettings Get();
        void Save(ConnectionSettings settings);
    }
}
