using Server.Models.Interfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;

namespace Server.Services.Services
{
    public class HoldingsService
    {
        private readonly IHoldingRepository _holdingRepository;

        public HoldingsService(IHoldingRepository holdingRepository)
        {
            _holdingRepository = holdingRepository;
        }

        public async Task<IEnumerable<HoldingDto>> GetAll()
        {
            var holdings = await _holdingRepository.GetAll();
            return holdings.Select(HoldingMapper.MapToHoldingDto);
        }

        public async Task<HoldingDto> AddHolding(HoldingDto holding)
        {
            var newHolding = await _holdingRepository.Add(HoldingMapper.MapToHolding(holding));
            return HoldingMapper.MapToHoldingDto(newHolding);
        }

        public async Task<HoldingDto?> UpdateHolding(HoldingDto holding)
        {
            var updatedHolding = await _holdingRepository.Update(HoldingMapper.MapToHolding(holding));

            if (updatedHolding is null)
                return null;

            return HoldingMapper.MapToHoldingDto(updatedHolding);
        }

        public async Task<bool?> DeleteHolding(short eduYear)
        {
            return await _holdingRepository.Delete(eduYear);
        }
    }
}
