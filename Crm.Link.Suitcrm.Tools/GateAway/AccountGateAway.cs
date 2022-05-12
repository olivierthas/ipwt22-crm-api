using Crm.Link.Suitcrm.Tools.Models;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class AccountGateAway : GateAwayBase, IAccountGateAway
    {
        protected override string Module => "Accounts";
        public AccountGateAway(IHttpClientFactory httpClientFactory, TokenProvider tokenProvider)
        {
            HttpClient = httpClientFactory.CreateClient("Crm");
            Token = tokenProvider.GetToken();
        }

        public async override Task<HttpResponseMessage> CreateOrUpdate(ModuleModel moduleModel)
        {
            var content = await CreateContent(moduleModel);
            return await HttpClient!.PostAsync($"/api/v8/modules/{Module}", content);
        }
    }
}
