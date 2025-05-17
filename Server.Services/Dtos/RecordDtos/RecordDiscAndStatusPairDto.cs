using Server.Models.Enums;

namespace Server.Services.Dtos.RecordDtos;

public class RecordDiscAndStatusPairDto
{
    public string CodeName { get; set; } = null!;

    public RecordStatus Approved { get; set; }
}
