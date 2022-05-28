using Crm.Link.Suitcrm.Tools.Models;
using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class ContactGateAway : GateAwayBase, IContactGateAway
    {
        protected override string Module => "Contacts";
        public ContactGateAway(
            IHttpClientFactory httpClientFactory,
            TokenProvider tokenProvider) : base(tokenProvider)
        {
            HttpClient = httpClientFactory.CreateClient("Crm");
        }

        public async Task<ContactModel?> GetContact(string id)
        {
            var response = await HttpClient.GetAsync($"/Api/V8/module/{Module}/{id}");
            if (!response.IsSuccessStatusCode && string.IsNullOrWhiteSpace(await response.Content.ReadAsStringAsync()))
            {

                return null;
            }
            return JsonConvert.DeserializeObject<ContactModel>(await response.Content.ReadAsStringAsync());
        }
    }
}
