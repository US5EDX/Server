using Server.Models.Interfaces;
using Server.Models.Models;
using Server.Services.Dtos;

namespace Server.Services.Services;

public class AuditLogsService(IAuditLogRepository auditLogRepository)
{
    public async Task<int> GetCount(AuditFiltersDto auditFiltersDto) =>
        await auditLogRepository.GetCount(auditFiltersDto.ActionType, auditFiltersDto.LeftBorder, auditFiltersDto.RightBorder,
            auditFiltersDto.HasDescription);

    public async Task<IReadOnlyList<Auditlog>> Get(int page, int size, AuditFiltersDto auditFiltersDto) =>
        await auditLogRepository.Get(page, size, auditFiltersDto.ActionType, auditFiltersDto.LeftBorder, auditFiltersDto.RightBorder,
            auditFiltersDto.HasDescription);
}
