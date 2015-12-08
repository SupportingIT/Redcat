using Redcat.Core;

namespace Redcat.Communicator.Services
{
    public interface IProtocolUiService
    {
        void ShowNewSettingsEditor();
        void ShowSettingsEditor(ConnectionSettings settings);
    }
}
