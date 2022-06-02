using Crm.Link.Suitcrm.Tools.Models;
using Microsoft.Extensions.Logging;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class AccountGateAway : GateAwayBase<ModuleModel, ModuleModel>, IAccountGateAway
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
