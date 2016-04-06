using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Xmpp
{
    public class RosterModule
    {
        private ICollection<RosterItem> roster;
        private Action<Stanza> stanzaSender;
        
        public RosterModule(ICollection<RosterItem> roster, Action<Stanza> stanzaSender)
        {
            this.roster = roster;
            this.stanzaSender = stanzaSender;
        }

        public void RequestRosterItems()
        {
            stanzaSender(Roster.Request());
        }

        public void AddRosterItem(JID jid, string name = null)
        {
            stanzaSender(Roster.AddItem(jid, name));
        }

        public void RemoveRosterItem(JID jid)
        {
            stanzaSender(Roster.RemoveItem(jid));
        }        

        public void OnIqStanzaReceived(IqStanza value)
        {
            if (!value.IsRosterIq()) return;

            if (value.IsRosterPush())
            {
                var item = ParseItem(value.GetRosterItems().First());
                roster.Add(item);
            }

            if (value.IsResult())
            {
                var items = value.GetRosterItems().Select(i => ParseItem(i));
                roster.Clear();
                foreach (var item in items) roster.Add(item);
            }            
        }

        private RosterItem ParseItem(XmlElement element)
        {
            RosterItem item = new RosterItem();
            item.Name = element.GetAttributeValue<string>("name");
            item.SubscriptionState = ParseSubscriptionState(element.GetAttributeValue<string>("subscription"));
            object jid = element.GetAttributeValue<object>("jid");

            if (jid != null)
            {
                if (jid is JID) item.Jid = (JID)jid;
                if (jid is string) item.Jid = (string)jid;
            }

            return item;
        }

        private SubscriptionState ParseSubscriptionState(string state)
        {
            if (state == "from") return SubscriptionState.From;
            if (state == "to") return SubscriptionState.To;
            if (state == "both") return SubscriptionState.Both;
            return SubscriptionState.None;
        }
    }
}
