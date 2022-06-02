using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class ContactData
    {
        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("attributes")]
        public ContactModel Attributes { get; set; }
    }
}