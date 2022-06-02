using Crm.Link.Suitcrm.Tools.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class SessionGateAway : GateAwayBase<MeetingBaseObject, MeetingBaseObject>, ISessionGateAway
    {
        protected override string Module => "Meetings";

        public SessionGateAway(
            IHttpClientFactory httpClientFactory,
            TokenProvider tokenProvider,
            ILogger<SessionGateAway> logger) : base(tokenProvider, logger)
        {
            this.HttpClient = httpClientFactory.CreateClient("Crm");
        }

        public async Task AddUserToSession(string module, string userId, string sessionId)
        {
            var body = new
            {
                data = new
                {
                    type = module,
                    id = userId
                }
            };

            HttpClient!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var json = JsonConvert.SerializeObject(body);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            stringContent.Headers.ContentType!.CharSet = "";

            await HttpClient.PostAsync("/Api/V8/module/Meetings/relationships/", stringContent);
        }

        public async Task<MeetingContacts?> GetContactsInMeeting(string meetingId)
        {
            var response = await HttpClient.GetAsync("/Api/V8/module/Meetings/{meetingId/relationships/contacts}");

            if (!response.IsSuccessStatusCode)
                return null;

            var contentJson = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("response createOrUpdate: {resp}", contentJson);
            return JsonConvert.DeserializeObject<MeetingContacts>(contentJson);
        }

        public async Task RemoveUserFromSession(string module, string userId, string sessionId)
        {
            HttpClient!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            await HttpClient.DeleteAsync($"/Api/V8/module/Meetings/{sessionId}/relationships/{module}/{userId}");
        }
    }
}
