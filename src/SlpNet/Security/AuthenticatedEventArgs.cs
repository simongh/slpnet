using System;

namespace Discovery.Slp.Security
{
    public class AuthenticatedEventArgs : EventArgs
    {
        public ServiceEntry ServiceEntry
        {
            get;
            private set;
        }

        public AuthenticationBlock Authentication
        {
            get;
            private set;
        }

        internal AuthenticatedEventArgs(ServiceEntry serviceEntry, AuthenticationBlock auth)
            : base()
        {
            ServiceEntry = serviceEntry;
            Authentication = auth;
        }
    }
}
