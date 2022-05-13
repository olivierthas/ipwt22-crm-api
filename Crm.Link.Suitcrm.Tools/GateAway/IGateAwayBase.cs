using Crm.Link.Suitcrm.Tools.Models;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public interface IGateAwayBase<T> where T : ModuleModel
    {
        Task<HttpResponseMessage> CreateOrUpdate(ModuleModel moduleModel);
        Task<HttpResponseMessage> Delete(Guid id);
    }
}