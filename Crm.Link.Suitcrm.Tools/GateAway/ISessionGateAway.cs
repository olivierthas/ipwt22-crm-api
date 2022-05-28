using Crm.Link.Suitcrm.Tools.Models;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public interface ISessionGateAway : IGateAwayBase
    {
        Task AddUserToSession(string module, string userId, string sessionId);
        Task RemoveUserFromSession(string module, string userId, string sessionId);
    }
}