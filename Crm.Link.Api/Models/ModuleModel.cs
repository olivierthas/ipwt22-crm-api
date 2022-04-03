using Newtonsoft.Json;

namespace Crm.Link.Api.Models
{
    public class ModuleModel
    {
        [JsonProperty("data")]
        public BaseModel Data { get; set; }
    }
}
