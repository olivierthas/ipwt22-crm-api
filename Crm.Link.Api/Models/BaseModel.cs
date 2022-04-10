﻿using Newtonsoft.Json;

namespace Crm.Link.Api.Models
{
    public class BaseModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public ICrmModel Attributes { get; set; }
    }
}