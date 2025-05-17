using Server.Models.Enums;
using Server.Models.Models;

namespace Server.Models.Interfaces;

public interface IStudentRepository
{
    Task<IReadOnlyList<Student>> GetWithLastReocorsByGroupId(uint groupId, short holding);
    Task<User> Add(User user);
    Task<IReadOnlyList<User>> AddRange(List<User> users, IEnumerable<Student> studentPart);
    Task<Student?> Update(Student student);
    Task<DeleteResultEnum> Delete(byte[] userId, Roles requestUserRole, short deleteBorderYear);
}