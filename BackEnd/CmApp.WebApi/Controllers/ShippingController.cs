using CmApp.Contracts.Entities;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
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
        private readonly IShippingRepository shippingRepository;
        private readonly ICarRepository carRepo;
        private readonly IShippingService shippingService;

        public ShippingController(IShippingRepository shippingRepository, ICarRepository carRepo, 
            IShippingService shippingService)
        {
            this.shippingRepository = shippingRepository;
            this.carRepo = carRepo;
            this.shippingService = shippingService;
        }


        // GET: api/cars/{carId}/shipping
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Get(int carId)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
               /* if (car.User != userId)
                    throw new Exception("Car does not exist");*/

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
        public async Task<IActionResult> Post(int carId, [FromBody] Shipping shipping)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var userCurrency = HttpContext.User.FindFirst(ClaimTypes.UserData).Value;
                //shipping.BaseCurrency = userCurrency;
                var car = await carRepo.GetCarById(carId);
                /*if (car.User != userId)
                    throw new Exception("Car does not exist");*/

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
        public async Task<IActionResult> Put(int carId, [FromBody] Shipping shipping)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var userCurrency = HttpContext.User.FindFirst(ClaimTypes.UserData).Value;
                //shipping.BaseCurrency = userCurrency;
                var car = await carRepo.GetCarById(carId);
              /*  if (car.User != userId)
                    throw new Exception("Car does not exist");*/

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
        public async Task<IActionResult> Delete(int carId)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
               /* if (car.User != userId)
                    throw new Exception("Car does not exist");*/

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
