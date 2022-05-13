using Crm.Link.Suitcrm.Tools.Models;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public interface IGateAwayBase<T> where T : ICrmModel
    {
        Task<HttpResponseMessage> CreateOrUpdate(T moduleModel);
        Task<HttpResponseMessage> Delete(Guid id);
    }
}