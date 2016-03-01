using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void RequestRosterItems()
        {
            channel.Send(Roster.Request());
        }

        public void AddRosterItem(JID jid, string name = null)
        {
            channel.Send(Roster.AddItem(jid, name));
        }

        public void RemoveRosterItem(JID jid)
        {
            channel.Send(Roster.RemoveItem(jid));
        }

        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }

        public void OnNext(IqStanza value)
        {
            if (value.IsRosterPush())
            {
                var item = ParseItem(value.GetRosterItems().First());
                //Need to think a little bit
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
