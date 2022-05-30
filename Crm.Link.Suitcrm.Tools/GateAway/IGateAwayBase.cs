using Crm.Link.Suitcrm.Tools.Models;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public interface IGateAwayBase
    {
        Task<Response?> CreateOrUpdate(ModuleModel moduleModel);
        Task<HttpResponseMessage> Delete(string id);
    }
}