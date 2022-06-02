using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class Data
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attributes")]
        public ContactModel Contact { get; set; }
    }
}