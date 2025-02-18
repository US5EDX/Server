using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IDisciplineRepository
    {
        Task<Discipline> Add(Discipline discipline);
        Task<bool?> Delete(uint disciplineId);
        Task<Discipline?> GetById(uint disciplineId);
        Task<int> GetCount(uint facultyId, short eduYear);
        Task<int> GetCount(uint facultyId, short eduYear, byte catalogType);
        Task<IEnumerable<Discipline>> GetDisciplines(int pageNumber, int pageSize, uint facultyId, short eduYear);
        Task<IEnumerable<Discipline>> GetDisciplines(int pageNumber, int pageSize, uint facultyId, short eduYear, byte catalogType);
        Task<Discipline?> Update(Discipline discipline);
        Task<bool> UpdateStatus(uint disciplineId);
    }
}