using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CmApp.Contracts;
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
        private readonly ITrackingService trackingService = new TrackingService()
        {
            TrackingRepository = new TrackingRepository(),
            CarRepository = new CarRepository(),
            ScraperService = new ScraperService(),
            FileRepository = new FileRepository()
        };
        private readonly ICarRepository carRepo = new CarRepository();

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
                if (tracking == null)
                    throw new BusinessException("There is no tracking info yet");
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

                var newTracking = await trackingService.LookForTrackingData(carId);
                return Ok(newTracking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // POST: api/cars/{carId}/trackingImages
        [HttpGet]
        [Route("/api/cars/{carId}/tracking-images")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> GetTrackingImages(string carId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId && role != "admin")
                    throw new Exception("Car does not exist");

                var newTracking = await trackingService.LookForTrackingImages(carId);
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
