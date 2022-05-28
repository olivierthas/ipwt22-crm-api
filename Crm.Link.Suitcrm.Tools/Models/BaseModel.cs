using Newtonsoft.Json;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class BaseModel
    {
        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("attributes")]
        public ICrmModel? Attributes { get; set; }

        public override string ToString()
        {
            return $"{Type}, {Attributes.ToString()}";
        }
    }
}
