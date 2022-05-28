using Crm.Link.Suitcrm.Tools.Models;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class AccountGateAway : GateAwayBase, IAccountGateAway
    {
        protected override string Module => "Accounts";
        public AccountGateAway(
            IHttpClientFactory httpClientFactory,
            TokenProvider tokenProvider)
            : base (tokenProvider)
        {
            HttpClient = httpClientFactory.CreateClient("Crm");
        }               
    }
}
