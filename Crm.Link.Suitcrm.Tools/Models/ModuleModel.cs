using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class ModuleModel
    {
        [JsonProperty("data")]
        public BaseModel? Data { get; set; }

        public override string ToString()
        {
            return Data?.ToString();
        }
    }
}
