using CmApp.Domains;
using CmApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ICarService
    {
        Task<CarEntity> InsertCar(CarEntity car);
        Task DeleteCar(string userId, string carId);
        Task UpdateCar(string userId, string carId, CarEntity car);

        //summary
        Task UpdateSoldSummary(string carId, SummaryEntity summary);
        Task<SummaryEntity> InsertCarSummary(string carId, SummaryEntity summary);

        //shipping
        Task UpdateShipping(string carId, ShippingEntity shipping);
        Task<ShippingEntity> InsertShipping(string carId, ShippingEntity shipping);
    }
}
