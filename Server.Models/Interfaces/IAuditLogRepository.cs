using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IAuditLogRepository
    {
        Task<IReadOnlyList<Auditlog>> Get(int page, int size, string actionType, DateTime leftBorder, DateTime rightBorder,
            bool hasDescription);
        Task<int> GetCount(string actionType, DateTime leftBorder, DateTime rightBorder, bool hasDescription);
    }
}