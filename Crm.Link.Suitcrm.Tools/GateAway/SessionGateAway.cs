﻿namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class SessionGateAway : GateAwayBase, ISessionGateAway
    {
        protected override string Module => "Meetings";

        public SessionGateAway(HttpClient httpClient, TokenProvider tokenProvider)
        {
            this.HttpClient = httpClient;
            Token = tokenProvider.GetToken();
        }

    }
}
