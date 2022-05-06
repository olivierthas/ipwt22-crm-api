using Newtonsoft.Json;

namespace Crm.Link.Api.Models
{
    public class AccountModel : ICrmModel
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("billing_address_street")]
        public string? Street { get; set; }

        [JsonProperty("billing_address_city")]
        public string? City { get; set; }

        [JsonProperty("billing_address_postalcode")]
        public string? PostalCode { get; set; }

        [JsonProperty("billing_address_country")]
        public string? Country { get; set; }

        [JsonProperty("phone_office")]
        public string? PhoneOffice { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        // public List<EmailAddress> Emails { get; set; }
    }
}
