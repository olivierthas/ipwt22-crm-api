using Crm.Link.Suitcrm.Tools.Models;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public interface ISessionGateAway : IGateAwayBase<MeetingModel>
    {
        Task AddUserToSession(string module, string userId, string sessionId);
    }
}