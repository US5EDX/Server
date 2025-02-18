﻿using Server.Models.Models;

namespace Server.Models.Interfaces
{
    public interface IHoldingRepository
    {
        Task<Holding> Add(Holding holding);
        Task<bool?> Delete(short eduYear);
        Task<IEnumerable<Holding>> GetAll();
        Task<IEnumerable<short>> GetLastFive();
        Task<Holding?> Update(Holding holding);
    }
}