using Crm.Link.Suitcrm.Tools.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class SessionGateAway : GateAwayBase<MeetingModel>, ISessionGateAway
    {
        protected override string Module => "Meetings";

        public SessionGateAway(
            IHttpClientFactory httpClientFactory,
            TokenProvider tokenProvider) : base(tokenProvider)
        {
            this.HttpClient = httpClientFactory.CreateClient("Crm");
        }

        public async Task AddUserToSession(string module, string userId, string sessionId)
        {
            var body = new
            {
                data = new {
                    type = module,
                    id = userId
                }
            };

            HttpClient!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var json = JsonConvert.SerializeObject(body);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");
            stringContent.Headers.ContentType!.CharSet = "";
            
            await HttpClient.PostAsync($"/api/v8/module/Meetings/relationships/", stringContent);
        }
    }
}
