using Redcat.Core;

namespace Redcat.App.Services
{
    public interface IProtocolUiService
    {
        void ShowNewSettingsEditor();
        void ShowSettingsEditor(ConnectionSettings settings);
    }
}
