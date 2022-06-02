using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class ContactBaseObject
    {
        [JsonProperty("data")]
        public ContactData Data { get; set; }
    }
}
