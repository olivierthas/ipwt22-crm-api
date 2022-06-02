using Crm.Link.Suitcrm.Tools.Models;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public interface ISessionGateAway : IGateAwayBase<MeetingBaseObject, MeetingBaseObject>
    {
        Task AddUserToSession(string module, string userId, string sessionId);
        Task<MeetingContacts?> GetContactsInMeeting(string meetingId);
        Task RemoveUserFromSession(string module, string userId, string sessionId);
    }
}