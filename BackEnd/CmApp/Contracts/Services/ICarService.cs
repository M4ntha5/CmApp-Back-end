using CmApp.Entities;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ICarService
    {
        Task<CarEntity> InsertCar(CarEntity car);
        Task DeleteCar(string userId, string carId);
        Task UpdateCar(string userId, string carId, CarEntity car);
    }
}
