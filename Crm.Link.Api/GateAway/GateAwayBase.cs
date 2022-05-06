using Crm.Link.Api.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Crm.Link.Api.GateAway
{
    public abstract class GateAwayBase : IGateAwayBase
    {
        protected abstract string Module { get; }

        protected HttpClient? HttpClient { get; set; }
        protected string? Token { get; set; }

        protected async Task<HttpContent> CreateContent(ModuleModel moduleModel)
        {
            HttpClient!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var json = JsonConvert.SerializeObject(moduleModel);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");
            stringContent.Headers.ContentType!.CharSet = "";
            return stringContent;
        }

        public virtual async Task<HttpResponseMessage> CreateOrUpdate(ModuleModel moduleModel)
        {
            var content = await CreateContent(moduleModel);
            return await HttpClient!.PostAsync($"/api/v8/modules/{Module}", content);
        }

        public virtual async Task<HttpResponseMessage> Delete(Guid id)
        {
            return await HttpClient!.DeleteAsync($"/api/v8/modules/{Module}/{id}");
        }
    }
}