namespace Crm.Link.UUID.Model
{
    public class ResourceDto
    {
        public Guid Uuid { get; set; }
        public string Source { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string SourceEntityId { get; set; }
        public int EntityVersion { get; set; }
    }
}
