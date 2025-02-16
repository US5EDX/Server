using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetByFacultyId(uint facultyId);
        Task<IEnumerable<Group>> GetByFacultyIdAndCodeFilter(uint facultyId, string codeFilter);
        Task<Group> Add(Group group);
        Task<Group?> Update(Group group);
        Task<bool?> Delete(uint groupId);
        Task UpdateGroupsCourse(uint facultyId);
    }
}