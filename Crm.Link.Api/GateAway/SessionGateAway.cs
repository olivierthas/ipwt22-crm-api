using Crm.Link.Api.Models;

namespace Crm.Link.Api.GateAway
{
    public class SessionGateAway : GateAwayBase, ISessionGateAway
    {
        protected override string module => "Meetings";
        
        public SessionGateAway(HttpClient httpClient, TokenProvider tokenProvider)
        {
            this.httpClient = httpClient;
            token = tokenProvider.GetToken();
        }
        
    }
}
