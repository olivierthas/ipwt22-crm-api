﻿namespace Crm.Link.Api.Models
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
        public string? StartDate { get; set; }

        [JsonProperty("date_end")]
        public string? EndDate { get; set; }

        [JsonProperty("outlook_ID")]
        public string? OutlookID { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }
    }
}
