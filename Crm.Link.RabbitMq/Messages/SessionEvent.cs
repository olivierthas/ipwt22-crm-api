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
        public string CrudMethode { get; set; }
        public int Version { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// UTC !!!!!!!!!
        /// </summary>
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string OrganiserId { get; set; }

    }
}
