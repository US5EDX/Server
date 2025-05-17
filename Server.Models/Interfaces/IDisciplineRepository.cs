using Server.Models.Enums;
using Server.Models.Models;

namespace Server.Models.Interfaces;

public interface IDisciplineRepository
{
    Task<Discipline?> GetById(uint disciplineId);
    Task<int> GetCount(uint facultyId, short eduYear, CatalogTypes? catalogType, Semesters? semesterFilter,
        byte[]? creatorFilter);
    Task<int> GetCountForStudent(EduLevels eduLevel, short holding, CatalogTypes catalogFilter,
        byte courseMask, Semesters semesterFilter, uint? facultyFilter);
    Task<Discipline> Add(Discipline discipline);
    Task<Discipline?> Update(Discipline discipline);
    Task<bool> UpdateStatus(uint disciplineId);
    Task<DeleteResultEnum> Delete(uint disciplineId, int notEnough);
}