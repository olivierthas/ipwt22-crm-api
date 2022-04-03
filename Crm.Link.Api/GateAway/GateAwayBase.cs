using Crm.Link.Api.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Crm.Link.Api.GateAway
{
    public abstract class GateAwayBase : IGateAwayBase
    {
        protected abstract string module { get; }

        protected HttpClient httpClient { get; set; }
        protected string token { get; set; }

        protected async Task<HttpContent> CreateContent(ModuleModel moduleModel)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(moduleModel);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");
            stringContent.Headers.ContentType!.CharSet = "";
            return stringContent;
        }

        public virtual async Task<HttpResponseMessage> CreateOrUpdate(ModuleModel moduleModel)
        {
            var content = await CreateContent(moduleModel);
            return await httpClient.PostAsync($"/api/v8/modules/{module}", content);
        }

        public virtual async Task<HttpResponseMessage> Delete(Guid id)
        {
            return await httpClient.DeleteAsync($"/api/v8/modules/{module}/{id}");
        }
    }
}