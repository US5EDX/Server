using Server.Services.Dtos.WorkerDtos;

namespace Server.Services.DtoInterfaces;

public interface IWorkerDtoRepository
{
    Task<IReadOnlyList<WorkerShortInfoDto>> GetByFacultyAndFullName(uint faculty, string fullName);
}