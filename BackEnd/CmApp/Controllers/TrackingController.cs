using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CmApp.Controllers
{
    [Route("api/cars/{carId}/tracking")]
    [Authorize(Roles = "user", AuthenticationSchemes = "user")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private static readonly ITrackingRepository trackingRepository = new TrackingRepository();
        private static readonly ICarRepository carRepository = new CarRepository();
        private readonly ITrackingService trackingService = new TrackingService()
        {
            TrackingRepository = trackingRepository,
            CarRepository = carRepository,
            ScraperService = new ScraperService(),
        };


        // GET: api/cars/{carId}/tracking
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Get(string carId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
                if (car.User != userId)
                    throw new Exception("Car does not exist");

                var tracking = await trackingRepository.GetTrackingByCar(carId);
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
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Post(string carId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
                if (car.User != userId)
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
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetTrackingImages(string carId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
                if (car.User != userId)
                    throw new Exception("Car does not exist");

                var newTracking = await trackingService.LookForTrackingImages(carId);
                return Ok(newTracking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //bring back if needed
        /*[Route("/api/cars/{carId}/tracking/download-images")]
        [Authorize(Roles = "user")]
        [HttpPost]
        public async Task<IActionResult> DownloadImages(string carId, [FromBody] List<string> images)
        {
            try
            {
                await trackingService.DownloadTrackingImages(carId, images);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

        // PUT: api/cars/{carId}/tracking/
        [HttpPut]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Put(string carId, [FromBody] TrackingEntity tracking)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
                if (car.User != userId)
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
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Delete(string carId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
                if (car.User != userId)
                    throw new Exception("Car does not exist");

                await trackingRepository.DeleteCarTracking(carId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("images/status")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> SaveLastShowImagesStatus(string carId, [FromBody] User data)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
                if (car.User != userId)
                    throw new Exception("Car does not exist");

                await trackingService.SaveLastShowImagesStatus(carId, data.Status);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
