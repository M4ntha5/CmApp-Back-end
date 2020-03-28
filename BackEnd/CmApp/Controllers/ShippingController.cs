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
    [Route("api/cars/{carId}/shipping")]
    [Authorize(Roles = "user, admin", AuthenticationSchemes = "user, admin")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly ShippingService shippingService = new ShippingService
        {
            ShippingRepository = new ShippingRepository(),
            SummaryRepository = new SummaryRepository()
        };
        private readonly CarRepository carRepo = new CarRepository();

        // GET: api/cars/{carId}/shipping
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

                var shipping = await shippingService.GetShipping(carId);
                return Ok(shipping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/cars/{carId}/shipping
        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Post(string carId, [FromBody] ShippingEntity shipping)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId && role != "admin")
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
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Put(string carId, [FromBody] ShippingEntity shipping)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId && role != "admin")
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

                await shippingService.DeleteShipping(carId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
