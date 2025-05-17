using Server.Models.Enums;
using Server.Models.Models;

namespace Server.Models.Interfaces;

public interface IGroupRepository
{
    Task<Group?> GetById(uint id);
    Task<IReadOnlyList<Group>> GetByFacultyAndCuratorId(uint facultyId, byte[]? curatorId);
    Task<Group?> GetGroupInfoByStudentId(byte[] studentId);
    Task<Group> Add(Group group);
    Task<Group?> Update(Group group);
    Task<DeleteResultEnum> Delete(uint groupId, DateTime currDate);
    Task<bool> DeleteGraduated(uint facultyId, DateTime currDate);
}