using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IGroupRepository
    {
        Task<Group?> GetById(uint id);
        Task<IEnumerable<Group>> GetByFacultyId(uint facultyId);
        Task<Group> Add(Group group);
        Task<Group?> Update(Group group);
        Task<bool?> Delete(uint groupId);
        Task UpdateGroupsCourse(uint facultyId);
    }
}