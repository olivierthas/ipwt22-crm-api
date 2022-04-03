using Crm.Link.Api.Models;

namespace Crm.Link.Api.GateAway
{
    public interface IGateAwayBase
    {
        Task<HttpResponseMessage> CreateOrUpdate(ModuleModel moduleModel);
        Task<HttpResponseMessage> Delete(Guid id);
    }
}