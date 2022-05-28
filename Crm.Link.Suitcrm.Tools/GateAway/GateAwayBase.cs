using Crm.Link.Suitcrm.Tools.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public abstract class GateAwayBase : IGateAwayBase
    {
        protected TokenProvider tokenProvider;
        protected abstract string Module { get; }
        protected HttpClient? HttpClient { get; set; }
        protected string? Token { get; set; }

        public GateAwayBase(TokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }
        protected HttpContent CreateContent(ModuleModel moduleModel)
        {
            HttpClient!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var json = JsonConvert.SerializeObject(moduleModel);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");
            stringContent.Headers.ContentType!.CharSet = "";
            return stringContent;
        }

        public virtual async Task<HttpResponseMessage> CreateOrUpdate(ModuleModel moduleModel)
        {
            CheckToken();  
            var content = CreateContent(moduleModel);
            return await HttpClient!.PostAsync($"/api/v8/modules/{Module}", content);
        }

        public virtual async Task<HttpResponseMessage> Delete(string id)
        {
            CheckToken();
            return await HttpClient!.DeleteAsync($"/api/v8/modules/{Module}/{id}");
        }

        protected void CheckToken()
        {
            if (Token == null)
                Token = tokenProvider.GetToken();
        }
    }
}