using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IStudentRepository
    {
        Task<User> Add(User user);
        Task<bool?> Delete(byte[] userId, int requestUserRole);
        Task<IEnumerable<Student>> GetWithLastReocorsByGroupId(uint groupId, short holding);
        Task<Student?> Update(Student student);
    }
}