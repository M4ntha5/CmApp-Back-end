using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ICarMakesRepository
    {
        Task<List<CarMakesEntity>> GetAllMakes();
        Task<CarMakesEntity> InsertCarMake(CarMakesEntity make);
        Task UpdateCarMake(CarMakesEntity make);
        Task DeleteCarMake(string makeId);
    }
}
