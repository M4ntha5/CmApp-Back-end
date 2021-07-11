using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.WebApi.Controllers
{
    [Route("api/cars/{carId}/tracking")]
    [Authorize(Roles = "user", AuthenticationSchemes = "user")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingRepository _trackingRepository;
        private readonly ICarRepository _carRepository;
        private readonly ITrackingService _trackingService;

        public TrackingController(ITrackingRepository trackingRepository, ICarRepository carRepository, 
            ITrackingService trackingService)
        {
            _trackingRepository = trackingRepository;
            _carRepository = carRepository;
            _trackingService = trackingService;
        }


        // GET: api/cars/{carId}/tracking
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Get(int carId)
        {
            try
            {
                var tracking = await _trackingRepository.GetTrackingByCar(carId);
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
        public async Task<IActionResult> Post(int carId)
        {
            try
            {
                var newTracking = await _trackingService.LookForTrackingData(carId);
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
        public async Task<IActionResult> GetTrackingImages(int carId)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = _carRepository.GetCarById(carId);
               /* if (car.User != userId)
                    throw new Exception("Car does not exist");*/

                var newTracking = await _trackingService.LookForTrackingImages(carId);
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
        public async Task<IActionResult> Put(int carId, [FromBody] Tracking tracking)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = _carRepository.GetCarById(carId);
               /* if (car.User != userId)
                    throw new Exception("Car does not exist");*/

                await _trackingService.UpdateTracking(carId, tracking);
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
        public async Task<IActionResult> Delete(int carId)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = _carRepository.GetCarById(carId);
               /* if (car.User != userId)
                    throw new Exception("Car does not exist");*/

                await _trackingRepository.DeleteCarTracking(carId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("images/status")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> SaveLastShowImagesStatus(int carId, [FromBody] Contracts.DTO.User data)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = _carRepository.GetCarById(carId);
               /* if (car.User != userId)
                    throw new Exception("Car does not exist");*/

                await _trackingService.SaveLastShowImagesStatus(carId, data.Status);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
