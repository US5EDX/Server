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
        Task<List<Discipline>> GetShortInfoByCodeEduYearEduLevelSemester(string code, short eduYear, byte eduLevel, byte semester);
        Task<Discipline?> Update(Discipline discipline);
        Task<bool> UpdateStatus(uint disciplineId);
    }
}