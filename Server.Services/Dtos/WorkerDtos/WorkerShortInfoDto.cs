using Server.Services.Converters;

namespace Server.Services.Dtos.WorkerDtos;

public class WorkerShortInfoDto
{
    public string WorkerId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public WorkerShortInfoDto() { }

    public WorkerShortInfoDto(byte[] id, string fullName)
    {
        WorkerId = UlidConverter.ByteIdToString(id);
        FullName = fullName;
    }
}
