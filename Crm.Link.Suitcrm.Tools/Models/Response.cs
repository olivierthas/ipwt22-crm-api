using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class Response
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
