using Redcat.Xmpp;

namespace Redcat.App.ViewModels
{
    public class PresenceStatusViewModel
    {
        public PresenceStatusViewModel(string displayText, PresenceStatus status)
        {
            DisplayText = displayText;
            Status = status;
        }

        public string DisplayText { get; }
        public PresenceStatus Status { get; }
    }
}
