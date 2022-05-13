using Crm.Link.Suitcrm.Tools.Models;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class SessionGateAway : GateAwayBase<MeetingModel>, ISessionGateAway
    {
        protected override string Module => "Meetings";

        public SessionGateAway(HttpClient httpClient, TokenProvider tokenProvider)
        {
            this.HttpClient = httpClient;
            Token = tokenProvider.GetToken();
        }

    }
}
