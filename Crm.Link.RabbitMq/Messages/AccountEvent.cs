using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Link.RabbitMq.Producer
{
    public class AccountEvent
    {
        /// <summary>
        /// UUID from UUIDMaster
        /// </summary>
        public string UUId { get; set; }
        /// <summary>
        /// Create, Update, Delete
        /// </summary>
        public string CrudMethode { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> SessionEventIds { get; set; } // tocheck
        public IEnumerable<string> OrganiserForSessionIds { get; set; } // tocheck
    }
}
