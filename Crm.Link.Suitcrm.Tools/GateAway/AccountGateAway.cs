using Crm.Link.Suitcrm.Tools.Models;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class AccountGateAway : GateAwayBase<AccountModel>, IAccountGateAway
    {
        protected override string Module => "Accounts";
        public AccountGateAway(IHttpClientFactory httpClientFactory, TokenProvider tokenProvider)
        {
            HttpClient = httpClientFactory.CreateClient("Crm");
            Token = tokenProvider.GetToken();
        }               
    }
}
