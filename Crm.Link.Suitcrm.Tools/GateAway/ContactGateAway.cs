using Crm.Link.Suitcrm.Tools.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class ContactGateAway : GateAwayBase<ContactBaseObject, ContactBaseObject>, IContactGateAway
    {
        protected override string Module => "Contacts";
        public ContactGateAway(
            IHttpClientFactory httpClientFactory,
            TokenProvider tokenProvider,
            ILogger<ContactGateAway> logger) : base(tokenProvider, logger)
        {
            HttpClient = httpClientFactory.CreateClient("Crm");
        }

        public async Task<ContactBaseObject?> GetContact(string id)
        {
            var response = await HttpClient.GetAsync($"/Api/V8/module/{Module}/{id}");
            if (!response.IsSuccessStatusCode && string.IsNullOrWhiteSpace(await response.Content.ReadAsStringAsync()))
            {

                return null;
            }
            return JsonConvert.DeserializeObject<ContactBaseObject>(await response.Content.ReadAsStringAsync());
        }
    }
}
