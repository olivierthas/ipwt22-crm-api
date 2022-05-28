using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class MeetingModel : ICrmModel
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("location")]
        public string? Location { get; set; }

        [JsonProperty("date_start")]
        public DateTime StartDate { get; set; }

        [JsonProperty("date_end")]
        public DateTime EndDate { get; set; }

        [JsonProperty("outlook_ID")]
        public string? OutlookID { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Name}, {Description}, {Location}, {StartDate}, {EndDate}, {OutlookID}, {Status}";
        }
    }
}
