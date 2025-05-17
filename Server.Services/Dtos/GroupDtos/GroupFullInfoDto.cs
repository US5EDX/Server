using Server.Models.Enums;
using Server.Services.Dtos.WorkerDtos;

namespace Server.Services.Dtos.GroupDtos;

public class GroupFullInfoDto
{
    public uint? GroupId { get; set; }

    public string GroupCode { get; set; } = null!;

    public SpecialtyDto Specialty { get; set; } = null!;

    public EduLevels EduLevel { get; set; }

    public byte Course { get; set; }

    public byte DurationOfStudy { get; set; }

    public short AdmissionYear { get; set; }

    public byte Nonparsemester { get; set; }

    public byte Parsemester { get; set; }

    public bool HasEnterChoise { get; set; }

    public byte ChoiceDifference { get; set; }

    public WorkerShortInfoDto? CuratorInfo { get; set; }
}
