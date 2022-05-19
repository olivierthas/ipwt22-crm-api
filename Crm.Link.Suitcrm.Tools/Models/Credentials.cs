using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class Credentials
    {
        [JsonProperty("grant_type")]
        public string? GrantType { get; set; }

        [JsonProperty("client_id")]
        public string? ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string? ClientSecret { get; set; }

        [JsonProperty("scope")]
        public string? Scope { get; set; }
    }
}
