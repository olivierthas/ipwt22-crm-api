﻿namespace Crm.Link.Suitcrm.Tools.Models
{
    public class MeetingModel : ICrmModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
