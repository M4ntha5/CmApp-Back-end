using CmApp.Contracts;
using CmApp.Entities;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class ShippingService : IShippingService
    {
        public IShippingRepository ShippingRepository { get; set; }

        public async Task DeleteShipping(string carId)
        {
            await ShippingRepository.DeleteCarShipping(carId);
        }
        public async Task UpdateShipping(string carId, ShippingEntity shipping)
        {
            shipping.Car = carId;
            var shippingId = ShippingRepository.GetShippingByCar(carId).Result.Id;
            await ShippingRepository.UpdateCarShipping(shippingId, shipping);
        }
        public async Task<ShippingEntity> GetShipping(string carId)
        {
            var shipping = await ShippingRepository.GetShippingByCar(carId);
            return shipping;
        }
        public async Task<ShippingEntity> InsertShipping(string carId, ShippingEntity shipping)
        {
            shipping.Car = carId;
            var newTracking = await ShippingRepository.InsertShipping(shipping);
            return newTracking;
        }
    }
}
