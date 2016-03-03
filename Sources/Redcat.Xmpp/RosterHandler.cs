using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Redcat.Xmpp
{
    public class RosterHandler : IObserver<IqStanza>
    {
        private ICollection<RosterItem> roster;
        private IOutputChannel<XmlElement> channel;

        public RosterHandler(ICollection<RosterItem> roster, IOutputChannel<XmlElement> channel)
        {
            this.roster = roster;
            this.channel = channel;
        }

        public SynchronizationContext SyncContext { get; set; }

        public void RequestRosterItems()
        {
            channel.Send(Roster.Request());
        }

        public void AddRosterItem(JID jid, string name = null)
        {
            channel.Send(Roster.AddItem(jid, name));
        }

        public void RemoveRosterItem(JID jid, string name = null)
        {
            channel.Send(Roster.RemoveItem(jid, name));
        }

        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }

        public void OnNext(IqStanza stanza)
        {
            if (SyncContext != null) SyncContext.Send(s => OnNextIq(stanza), null);
            else OnNextIq(stanza);         
        }

        private void OnNextIq(IqStanza stanza)
        {
            if (stanza.IsRosterPush())
            {
                var item = ParseItem(stanza.GetRosterItems().First());
                roster.Add(item);
            }

            if (stanza.IsRosterResponse())
            {
                var items = stanza.GetRosterItems().Select(i => ParseItem(i));
                roster.Clear();
                foreach (var item in items) roster.Add(item);
            }
        }

        private RosterItem ParseItem(XmlElement element)
        {
            RosterItem item = new RosterItem();
            item.Name = element.GetAttributeValue<string>("name");
            object jid = element.GetAttributeValue<object>("jid");

            if (jid != null)
            {
                if (jid is JID) item.Jid = (JID)jid;
                if (jid is string) item.Jid = (string)jid;
            }

            return item;
        }
    }
}
