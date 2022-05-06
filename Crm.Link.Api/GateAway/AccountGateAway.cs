using Crm.Link.Api.Models;

namespace Crm.Link.Api.GateAway
{
    public class AccountGateAway : GateAwayBase, IAccountGateAway
    {
        protected override string Module => "Accounts";
        public AccountGateAway(IHttpClientFactory httpClientFactory, TokenProvider tokenProvider)
        {
            HttpClient = httpClientFactory.CreateClient("Crm");
            Token = tokenProvider.GetToken();
        }

        // moet weg
        public async override Task<HttpResponseMessage> CreateOrUpdate(ModuleModel moduleModel)
        {           
            
            var content = await CreateContent(moduleModel);
            return await HttpClient!.PostAsync($"/api/v8/modules/{Module}", content);
        }
    }
}
