using CmApp.Entities;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IShippingService
    {
        Task UpdateShipping(string carId, ShippingEntity shipping);
        Task<ShippingEntity> InsertShipping(string carId, ShippingEntity shipping);

    }
}
