using Crm.Link.RabbitMq.Messages;

namespace Crm.Link.RabbitMq.Producer
{
    public class SessionEvent
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
        public string Title { get; set; }

        /// <summary>
        /// UTC !!!!!!!!!
        /// </summary>
        public DateTime StartDateUTC { get; set; }
        public DateTime EndDateUTC { get; set; }
        public string Description { get; set; }
        public string? OrganiserUUId { get; set; }
        public bool IsActive { get; set; }

    }
}
