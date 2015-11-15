using Redcat.Core;
using Redcat.Xmpp.Negotiators;
using System;

namespace Redcat.Xmpp
{
    public class StreamInitializerFactory : IStreamInitializerFactory
    {
        private Action setTlsContext;

        public StreamInitializerFactory(Action setTlsContext)
        {
            if (setTlsContext == null) throw new ArgumentNullException(nameof(setTlsContext));
            this.setTlsContext = setTlsContext;
        }

        public IStreamInitializer CreateInitializer(ConnectionSettings settings)
        {
            StreamInitializer initializer = new StreamInitializer(settings);
            TlsNegotiator tls = new TlsNegotiator(setTlsContext);
            SaslNegotiator sasl = new SaslNegotiator();
            sasl.AddAuthenticator("PLAIN", Authenticators.Plain);
            initializer.Negotiators.Add(tls);
            initializer.Negotiators.Add(sasl);
            return initializer;
        }
    }
}
