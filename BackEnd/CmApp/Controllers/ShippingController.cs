using System.Threading.Tasks;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{
    [Route("api/cars/{carId}/shipping")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly ShippingService shippingService = new ShippingService
        {
            ShippingRepository = new ShippingRepository(),
            SummaryRepository = new SummaryRepository()
        };

        // GET: api/cars/{carId}/shipping
        [HttpGet]
        public async Task<ShippingEntity> Get(string carId)
        {
            var shipping = await shippingService.GetShipping(carId);
            return shipping;
        }

        // POST: api/cars/{carId}/shipping
        [HttpPost]
        public async Task<ShippingEntity> Post(string carId, [FromBody] ShippingEntity shipping)
        {
            var newShipping = await shippingService.InsertShipping(carId, shipping);
            return newShipping;
        }

        // PUT: api/cars/{carId}/shipping
        [HttpPut]
        public async Task<NoContentResult> Put(string carId, [FromBody] ShippingEntity shipping)
        {
            await shippingService.UpdateShipping(carId, shipping);
            return NoContent();
        }

        // DELETE: api/cars/{carId}/shipping
        [HttpDelete]
        public async Task<NoContentResult> Delete(string carId)
        {
            await shippingService.DeleteShipping(carId);
            return NoContent();
        }
    }
}
