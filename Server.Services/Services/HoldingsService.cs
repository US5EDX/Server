using Server.Models.CustomExceptions;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;

namespace Server.Services.Services
{
    public class HoldingsService(IHoldingRepository holdingRepository)
    {
        public async Task<IEnumerable<HoldingDto>> GetAll()
        {
            var holdings = await holdingRepository.GetAll();
            return holdings.Select(HoldingMapper.MapToHoldingDto);
        }

        public async Task<IReadOnlyList<short>> GetLastFive() => await holdingRepository.GetLastNYears(5);

        public async Task<HoldingDto> GetLast()
        {
            var lastHolding = await holdingRepository.GetLast();
            return HoldingMapper.MapToHoldingDto(lastHolding);
        }

        public async Task<HoldingDto> AddHolding(HoldingDto holding)
        {
            var newHolding = await holdingRepository.Add(HoldingMapper.MapToHolding(holding));
            return HoldingMapper.MapToHoldingDto(newHolding);
        }

        public async Task<HoldingDto> UpdateOrThrow(HoldingDto holding)
        {
            var updatedHolding = await holdingRepository.Update(HoldingMapper.MapToHolding(holding)) ??
                throw new NotFoundException("Спеціальність не знайдено");

            return HoldingMapper.MapToHoldingDto(updatedHolding);
        }

        public async Task DeleteOrThrow(short eduYear)
        {
            var result = await holdingRepository.Delete(eduYear);

            result.ThrowIfFailed("Вказаний навчальний рік не знайдено",
                "Неможливо видалити, оскільки до навчального року є прив'язані дані");
        }
    }
}
