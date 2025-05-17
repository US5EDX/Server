namespace Server.Services.Dtos.DisciplineDtos;

public class DisciplineStatusColors
{
    public string NotEnough { get; set; } = string.Empty;

    public string PartiallyFilled { get; set; } = string.Empty;

    public string Filled { get; set; } = string.Empty;

    public string GetColor(int studentsCount, DisciplineStatusThresholds thresholds)
    {
        if (studentsCount < thresholds.NotEnough) return NotEnough;

        if (studentsCount < thresholds.PartiallyFilled) return PartiallyFilled;

        return Filled;
    }
}
