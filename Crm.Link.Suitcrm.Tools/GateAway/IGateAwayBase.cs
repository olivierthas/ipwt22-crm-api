using Crm.Link.Suitcrm.Tools.Models;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public interface IGateAwayBase<TSend, TResponse>
    {
        Task<TResponse?> CreateOrUpdate(TSend moduleModel);
        Task<HttpResponseMessage> Delete(string id);
    }
}