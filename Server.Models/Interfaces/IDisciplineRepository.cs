using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IDisciplineRepository
    {
        Task<Discipline> Add(Discipline discipline);
        Task<bool?> Delete(uint disciplineId);
        Task<Discipline?> GetById(uint disciplineId);
        Task<int> GetCount(uint facultyId, short eduYear, byte? catalogType, byte? semesterFilter, byte[]? creatorFilter);
        Task<int> GetCountForStudent(byte eduLevel, short holding, byte catalogFilter, byte courseMask, byte semesterFilter, uint? facultyFilter);
        Task<Discipline?> Update(Discipline discipline);
        Task<bool> UpdateStatus(uint disciplineId);
    }
}