using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class MeetingContacts
    {
        [JsonProperty("data")]
        public List<Data> Data { get; set; }
    }
}
