using System;
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
            TrackingRepository = new TrackingRepository(),
            CarRepository = new CarRepository(),
            ScraperService = new WebScraper(),
            FileRepository = new FileRepository()
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
        public async Task<IActionResult> Post(string carId)
        {
            try
            {
                var newTracking = await trackingService.LookForTracking(carId);
                return StatusCode(200, newTracking);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }          
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
