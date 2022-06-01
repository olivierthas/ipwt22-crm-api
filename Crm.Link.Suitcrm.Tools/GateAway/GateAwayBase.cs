using Crm.Link.Suitcrm.Tools.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public abstract class GateAwayBase : IGateAwayBase
    {
        protected TokenProvider tokenProvider;
        protected readonly ILogger _logger;

        protected abstract string Module { get; }
        protected HttpClient? HttpClient { get; set; }
        protected string? Token { get; set; }

        public GateAwayBase(TokenProvider tokenProvider, ILogger logger)
        {
            this.tokenProvider = tokenProvider;
            _logger = logger;
        }
        protected HttpContent CreateContent(ModuleModel moduleModel)
        {
            HttpClient!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var json = JsonConvert.SerializeObject(moduleModel);
            _logger.LogInformation(json + "}");
            var stringContent = new StringContent(json + "}", Encoding.UTF8, "application/json");
            stringContent.Headers.ContentType!.CharSet = "UTF8";
            _logger.LogInformation(JsonConvert.SerializeObject(stringContent));
            return stringContent;
        }

        public virtual async Task<Response?> CreateOrUpdate(ModuleModel moduleModel)
        {
            CheckToken();  
            var content = CreateContent(moduleModel);
            var response = await HttpClient!.PostAsync($"/Api/V8/module", content);
            if (response.IsSuccessStatusCode)
            {
                var contentJson = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("response createOrUpdate: {resp}", contentJson);
                return JsonConvert.DeserializeObject<Response>(contentJson);
            }

            return null;
        }

        public virtual async Task<HttpResponseMessage> Delete(string id)
        {
            CheckToken();
            return await HttpClient!.DeleteAsync($"/Api/V8/module/{Module}/{id}");
        }

        protected void CheckToken()
        {
            if (Token == null)
                Token = tokenProvider.GetToken();
        }
    }
}