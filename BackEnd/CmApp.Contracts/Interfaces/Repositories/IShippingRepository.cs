using CmApp.Contracts.Entities;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface IShippingRepository
    {
        Task<ShippingEntity> InsertShipping(ShippingEntity shipping);
        Task DeleteCarShipping(int carId);
        Task UpdateCarShipping(int shippingId, ShippingEntity shipping);
        Task<ShippingEntity> GetShippingByCar(int carId);
    }
}
