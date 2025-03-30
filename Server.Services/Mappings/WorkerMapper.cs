using Server.Models.Models;
using Server.Services.Dtos;

namespace Server.Services.Mappings
{
    public class WorkerMapper
    {
        public static WorkerShortInfoDto MapToWorkerShortInfoDto(Worker worker)
        {
            return new WorkerShortInfoDto(worker.WorkerId, worker.FullName);
        }
    }
}
