using CmApp.Contracts.Entities;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface IShippingService
    {
        Task UpdateShipping(int carId, ShippingEntity shipping);
        Task<ShippingEntity> InsertShipping(int carId, ShippingEntity shipping);
    }
}
