using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ICarService
    {
        Task<CarEntity> InsertCarDetailsFromScraper(string vin);
    }
}
