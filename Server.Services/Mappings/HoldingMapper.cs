using Server.Models.Models;
using Server.Services.Dtos;

namespace Server.Services.Mappings;

public static class HoldingMapper
{
    public static HoldingDto MapToHoldingDto(Holding holding) =>
        new() { EduYear = holding.EduYear, StartDate = holding.StartDate, EndDate = holding.EndDate };

    public static Holding MapToHolding(HoldingDto holding) =>
        new() { EduYear = holding.EduYear, StartDate = holding.StartDate, EndDate = holding.EndDate };
}