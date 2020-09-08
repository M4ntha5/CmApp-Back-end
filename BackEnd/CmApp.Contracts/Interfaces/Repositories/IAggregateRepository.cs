using CmApp.Contracts.Domains;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface IAggregateRepository
    {
        Task<List<CarStats>> GetCarStats(DateTime dateFrom, DateTime dateTo, string userEmail);
        Task<List<CarDisplay>> GetUserCars(string userEmail);
    }
}
