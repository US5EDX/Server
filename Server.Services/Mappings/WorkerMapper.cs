using Server.Models.Models;
using Server.Services.Dtos.WorkerDtos;

namespace Server.Services.Mappings;

public class WorkerMapper
{
    public static WorkerShortInfoDto MapToWorkerShortInfoDto(Worker worker) => new(worker.WorkerId, worker.FullName);
}