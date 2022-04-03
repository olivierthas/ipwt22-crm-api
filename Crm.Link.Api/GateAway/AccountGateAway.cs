using Crm.Link.Api.Models;

namespace Crm.Link.Api.GateAway
{
    public class AccountGateAway : GateAwayBase, IAccountGateAway
    {
        protected override string module => "Accounts";
        public AccountGateAway(IHttpClientFactory httpClientFactory, TokenProvider tokenProvider)
        {
            httpClient = httpClientFactory.CreateClient("Crm");
            token = tokenProvider.GetToken();
        }

        // moet weg
        public async Task<HttpResponseMessage> CreateOrUpdate(ModuleModel moduleModel)
        {
            var data = new ModuleModel
            {
                Data = new BaseModel
                {
                    Type = "Accounts",
                    Attributes =
                            new AccountModel
                            {
                                Name = "Test user",
                                Street = "teststraat",
                                City = "blablabla",
                                PostalCode = "3111",
                                Country = "Belgium",
                                PhoneOffice = "+32 475 444 444",
                            }
                }
            };
            var content = await CreateContent(moduleModel);
            return await httpClient.PostAsync($"/api/v8/modules/{module}", content);
        }
    }
}
