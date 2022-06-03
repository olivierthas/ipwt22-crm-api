using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class ContactModel : ICrmModel
    {

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string? Id { get; set; }

        [JsonProperty("first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string? FirstName { get; set; }

        [JsonProperty("last_name", NullValueHandling = NullValueHandling.Ignore)]
        public string? LastName { get; set; }

        [JsonProperty("email1", NullValueHandling = NullValueHandling.Ignore)]
        public string? Email { get; set; }

        [JsonProperty("phone_mobile", NullValueHandling = NullValueHandling.Ignore)]
        public string? Phone { get; set; }
    }
}
