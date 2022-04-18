using Crm.Link.RabbitMq.Messages;

namespace Crm.Link.RabbitMq.Producer
{
    public class AttendeeEvent
    {
        /// <summary>
        /// UUID from UUIDMaster
        /// </summary>
        public string UUId { get; set; }
        /// <summary>
        /// Create, Update, Delete
        /// </summary>
        public MethodeEnum CrudMethode { get; set; }
        public int Version { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? VatNumber { get; set; } = null;        
    }
}
