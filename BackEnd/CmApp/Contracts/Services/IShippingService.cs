using CmApp.Entities;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IShippingService
    {
        Task DeleteShipping(string carId);
        Task UpdateShipping(string carId, ShippingEntity shipping);
        Task<ShippingEntity> GetShipping(string carId);
        Task<ShippingEntity> InsertShipping(string carId, ShippingEntity shipping);

    }
}
