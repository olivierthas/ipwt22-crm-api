using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Link.Suitcrm.Tools.Models
{
    public class ContactBaseObject
    {
        [JsonProperty("data")]
        public ContactData Data { get; set; }
    }
}
