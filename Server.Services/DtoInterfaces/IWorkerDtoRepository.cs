using Server.Services.Dtos;

namespace Server.Services.DtoInterfaces
{
    public interface IWorkerDtoRepository
    {
        Task<IEnumerable<WorkerShortInfoDto>> GetByFacultyAndFullName(uint faculty, string fullName);
    }
}