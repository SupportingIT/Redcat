using System.Collections.Generic;

namespace Redcat.Xmpp.Xml
{
    public class RosterItem : XmlElement
    {
        private ICollection<string> groups;

        public RosterItem(string itemName) : this(itemName, null)
        {}

        public RosterItem(string itemName, JID jid) : this(itemName, jid, "")
        {}

        public RosterItem(string itemName, JID jid, IEnumerable<string> groups) : this(itemName, jid)
        {
            foreach (string @group in groups) this.groups.Add(group);
        }

        public RosterItem(string itemName, JID jid, string subscription) : base("item")
        {
            groups = new List<string>();
            ItemName = itemName;
            Jid = jid;
            Subscription = subscription;
        }

        public string ItemName
        {
            get { return GetAttributeValue<string>("name"); }
            set { SetAttributeValue("name", value); }
        }

        public JID Jid
        {
            get { return GetAttributeValue<JID>("jid"); }
            set { SetAttributeValue("jid", value); }
        }

        public ICollection<string> Groups
        {
            get { return groups; }
        }

        public string Subscription
        {
            get { return GetAttributeValue<string>("subscription"); }
            set { SetAttributeValue("subscription", value); }
        }

        public void AddGroups(params string[] groupNames)
        {
            foreach (string groupName in groupNames) Groups.Add(groupName);
        }
    }
}
