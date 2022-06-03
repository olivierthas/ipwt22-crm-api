using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class MeetingModel : ICrmModel
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? Name { get; set; }
        
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string? Description { get; set; }

        [JsonProperty("date_start")]
        public DateTime StartDate { get; set; }

        [JsonProperty("date_end", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndDate { get; set; }

        [JsonProperty("duration_hours", NullValueHandling = NullValueHandling.Ignore)]
        public int DurationHours { get; set; } = 0;

        [JsonProperty("duration_minutes", NullValueHandling = NullValueHandling.Ignore)]
        public int DurationMinutes { get; set; } = 0;

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string? Status { get; set; }

        [JsonProperty("parent_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentType { get; set; }

        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentId { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Name}, {Description}, {StartDate}, {EndDate}, {Status}";
        }
    }
}
