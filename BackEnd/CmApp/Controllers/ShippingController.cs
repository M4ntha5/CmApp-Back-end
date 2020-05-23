using CmApp.Contracts;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CmApp.Controllers
{
    [Route("api/cars/{carId}/shipping")]
    [Authorize(Roles = "user", AuthenticationSchemes = "user")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private static readonly IShippingRepository shippingRepository = new ShippingRepository();
        private static readonly ICarRepository carRepo = new CarRepository();
        private readonly IShippingService shippingService = new ShippingService()
        {
            ExternalAPIService = new ExternalAPIService(),
            ShippingRepository = shippingRepository,
            SummaryRepository = new SummaryRepository()
        };

        // GET: api/cars/{carId}/shipping
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Get(string carId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId)
                    throw new Exception("Car does not exist");

                var shipping = await shippingRepository.GetShippingByCar(carId);
                return Ok(shipping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/cars/{carId}/shipping
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Post(string carId, [FromBody] ShippingEntity shipping)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId)
                    throw new Exception("Car does not exist");

                var newShipping = await shippingService.InsertShipping(carId, shipping);
                return Ok(newShipping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/cars/{carId}/shipping
        [HttpPut]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Put(string carId, [FromBody] ShippingEntity shipping)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId)
                    throw new Exception("Car does not exist");

                await shippingService.UpdateShipping(carId, shipping);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/cars/{carId}/shipping
        [HttpDelete]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Delete(string carId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId)
                    throw new Exception("Car does not exist");

                await shippingRepository.DeleteCarShipping(carId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
