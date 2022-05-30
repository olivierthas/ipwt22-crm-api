using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class AccountModel : ICrmModel
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? Name { get; set; }
        
        [JsonProperty("billing_address_street", NullValueHandling = NullValueHandling.Ignore)]
        public string? Street { get; set; }

        [JsonProperty("billing_address_city", NullValueHandling = NullValueHandling.Ignore)]
        public string? City { get; set; }

        [JsonProperty("billing_address_postalcode", NullValueHandling = NullValueHandling.Ignore)]
        public string? PostalCode { get; set; }

        [JsonProperty("billing_address_country", NullValueHandling = NullValueHandling.Ignore)]
        public string? Country { get; set; }

        [JsonProperty("phone_office", NullValueHandling = NullValueHandling.Ignore)]
        public string? PhoneOffice { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string? Email { get; set; }

        // public List<EmailAddress> Emails { get; set; }
    }
}
