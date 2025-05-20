using Server.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos.SettingDtos;

public class ThresholdValue
{
    [Required]
    [Range(0, 100)]
    public int NotEnough { get; set; }

    [Required]
    [Range(0, 100)]
    public int PartiallyFilled { get; set; }
}

public class Thresholds
{
    [Required]
    public ThresholdValue Bachelor { get; set; } = null!;

    [Required]
    public ThresholdValue Master { get; set; } = null!;

    [Required]
    public ThresholdValue PhD { get; set; } = null!;

    public ThresholdValue GetValue(EduLevels eduLevel) =>
        eduLevel switch
        {
            EduLevels.Bachelor => Bachelor,
            EduLevels.Master => Master,
            EduLevels.PHD => PhD,
            _ => throw new InvalidDataException(),
        };
}
