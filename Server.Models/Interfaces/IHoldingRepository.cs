using Server.Models.Enums;
using Server.Models.Models;

namespace Server.Models.Interfaces;

public interface IHoldingRepository
{
    Task<IReadOnlyList<Holding>> GetAll();
    Task<IReadOnlyList<short>> GetLastNYears(int limit);
    Task<short> GetLastYearAsync();
    Task<Holding> GetLast();
    Task<IReadOnlyList<short>> GetRequestedYears(HashSet<int> requestedYears);
    Task<Holding> Add(Holding holding);
    Task<Holding?> Update(Holding holding);
    Task<DeleteResultEnum> Delete(short eduYear);
}