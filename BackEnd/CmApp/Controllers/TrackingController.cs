using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{
    [Route("api/cars/{carId}/tracking")]
    [Authorize(Roles = "user, admin", AuthenticationSchemes = "user, admin")]
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
        private readonly CarRepository carRepo = new CarRepository();

        // GET: api/cars/{carId}/tracking
        [HttpGet]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Get(string carId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId && role != "admin")
                    throw new Exception("Car does not exist");

                var tracking = await trackingService.GetTracking(carId);
                return Ok(tracking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/cars/{carId}/tracking
        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Post(string carId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId && role != "admin")
                    throw new Exception("Car does not exist");

                var newTracking = await trackingService.LookForTracking(carId);
                return Ok(newTracking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/cars/{carId}/tracking/
        [HttpPut]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Put(string carId, [FromBody] TrackingEntity tracking)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId && role != "admin")
                    throw new Exception("Car does not exist");

                await trackingService.UpdateTracking(carId, tracking);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/cars/{carId}/tracking/
        [HttpDelete]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Delete(string carId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId && role != "admin")
                    throw new Exception("Car does not exist");

                await trackingService.DeleteTracking(carId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
