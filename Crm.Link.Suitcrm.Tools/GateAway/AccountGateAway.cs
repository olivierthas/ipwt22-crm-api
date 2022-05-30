using Microsoft.Extensions.Logging;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class AccountGateAway : GateAwayBase, IAccountGateAway
    {
        protected override string Module => "Accounts";
        public AccountGateAway(
            IHttpClientFactory httpClientFactory,
            TokenProvider tokenProvider,
            ILogger<AccountGateAway> logger)
            : base(tokenProvider, logger)
        {
            HttpClient = httpClientFactory.CreateClient("Crm");
        }
    }
}
