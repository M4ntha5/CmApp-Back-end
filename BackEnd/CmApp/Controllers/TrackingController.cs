using System.Threading.Tasks;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{
    [Route("api/cars/{carId}/tracking")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly TrackingService trackingService = new TrackingService()
        {
            TrackingRepository = new TrackingRepository()
        };

        // GET: api/cars/{carId}/tracking
        [HttpGet]
        public async Task<TrackingEntity> Get(string carId)
        {
            var tracking = await trackingService.GetTracking(carId);
            return tracking;
        }

        // POST: api/cars/{carId}/tracking
        [HttpPost]
        public async Task<TrackingEntity> Post(string carId, [FromBody] TrackingEntity tracking)
        {
            var newTracking = await trackingService.InsertTracking(carId, tracking);
            return newTracking;
        }

        // PUT: api/cars/{carId}/tracking/
        [HttpPut]
        public async Task<NoContentResult> Put(string carId, [FromBody] TrackingEntity tracking)
        {
            await trackingService.UpdateTracking(carId, tracking);
            return NoContent();
        }

        // DELETE: api/cars/{carId}/tracking/
        [HttpDelete]
        public async Task<NoContentResult> Delete(string carId)
        {
            await trackingService.DeleteTracking(carId);
            return NoContent();
        }
    }
}
