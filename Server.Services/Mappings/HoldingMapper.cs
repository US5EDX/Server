using Server.Models.Models;
using Server.Services.Dtos;

namespace Server.Services.Mappings
{
    public class HoldingMapper
    {
        public static HoldingDto MapToHoldingDto(Holding holding)
        {
            return new HoldingDto()
            {
                EduYear = holding.EduYear,
                StartDate = holding.StartDate,
                EndDate = holding.EndDate
            };
        }

        public static Holding MapToHolding(HoldingDto holding)
        {
            return new Holding()
            {
                EduYear = holding.EduYear,
                StartDate = holding.StartDate,
                EndDate = holding.EndDate
            };
        }
    }
}
