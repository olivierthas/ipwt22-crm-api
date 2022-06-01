using Crm.Link.Suitcrm.Tools.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
            stringContent.Headers.ContentType!.CharSet = "UTF-8";
            _logger.LogInformation(stringContent.ReadAsStringAsync().GetAwaiter().GetResult());            
            return stringContent;
        }

        public virtual async Task<Response?> CreateOrUpdate(ModuleModel moduleModel)
        {
            CheckToken();  
            var content = CreateContent(moduleModel);
            var json = JsonConvert.SerializeObject(moduleModel);
            var response = await HttpClient!.PostAsync($"/Api/V8/module", content);
            var response2 = await HttpClient!.PostAsJsonAsync($"/Api/V8/module", json + "}");
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