using Newtonsoft.Json;

namespace Crm.Link.Api.Models
{
    public class ContactModel : ICrmModel
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("first_name")]
        public string? FirstName { get; set; }

        [JsonProperty("last_name")]
        public string? LastName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }
    }
}