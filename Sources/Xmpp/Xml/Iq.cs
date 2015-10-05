using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Xmpp.Xml
{
    public static class Iq
    {
        public static class Type
        {
            public static readonly string Get = "get";
            public static readonly string Set = "set";
            public static readonly string Result = "result";
            public static readonly string Error = "error";
        }

        #region Creation methods

        public static IqStanza Get()
        {
            return new IqStanza(Type.Get);
        }

        public static IqStanza Set()
        {
            return new IqStanza(Type.Set);
        }

        public static IqStanza Result()
        {
            return new IqStanza(Type.Result);
        }

        public static IqStanza Error()
        {
            return new IqStanza(Type.Error);
        }

        #endregion

        #region Is.. methods

        public static bool IsGet(this IqStanza stanza)
        {
            return stanza.Type == Type.Get;
        }

        public static bool IsSet(this IqStanza stanza)
        {
            return stanza.Type == Type.Set;
        }

        public static bool IsResult(this IqStanza stanza)
        {
            return stanza.Type == Type.Result;
        }

        public static bool IsError(this IqStanza stanza)
        {
            return stanza.Type == Type.Error;
        }

        #endregion

        public static IqQuery Query(this IqStanza stanza)
        {
            throw new NotImplementedException();
        }

        public static IqStanza Query(this IqStanza stanza, string xmlns)
        {
            throw new NotImplementedException();
        }

        public static IqStanza Query(this IqStanza stanza, string xmlns, params XmlElement[] childElements)
        {
            throw new NotImplementedException();
        }

        public static IqStanza Query(this IqStanza stanza, string xmlns, Action<IqQuery> queryInit)
        {
            throw new NotImplementedException();
        }

        public static IqStanza RosterQuery(this IqStanza stanza)
        {
            throw new NotImplementedException();
        }

        public static IqStanza RosterQuery(this IqStanza stanza, params XmlElement[] childElements)
        {
            throw new NotImplementedException();
        }

        public static IqStanza RosterQuery(this IqStanza stanza, Action<IqQuery> queryInit)
        {
            throw new NotImplementedException();
        }

        public static bool HasQuery(this IqStanza stanza, string queryXmlns = "")
        {
            throw new NotImplementedException();
        }

        public static bool HasRosterQuery(this IqStanza stanza)
        {
            throw new NotImplementedException();
        }

        public static bool IsRosterResult(this IqStanza stanza)
        {
            return stanza.HasRosterQuery() && stanza.IsResult();
        }

        public static bool IsRosterPush(this IqStanza stanza)
        {
            return stanza.HasRosterQuery() && stanza.IsSet();
        }

        public static ICollection<RosterItem> RosterItems(this IqStanza stanza)
        {
            throw new NotImplementedException();
        }
    }
}
