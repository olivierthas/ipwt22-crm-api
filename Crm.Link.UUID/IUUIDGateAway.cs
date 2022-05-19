using Crm.Link.UUID.Model;

namespace Crm.Link.UUID
{
    public interface IUUIDGateAway
    {
        Task<ResourceDto?> GetGuid(string id, string sourceType, string entityType);
        Task<ResourceDto?> PublishEntity(string source, string entityType, string sourceEntityId, int version);
        Task<ResourceDto?> UpdateEntity(string id, string sourceType, string entityType);
    }
}