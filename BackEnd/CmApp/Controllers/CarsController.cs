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
    [Route("/api/cars")]
    [Authorize(Roles = "user, admin", AuthenticationSchemes = "user, admin")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private static readonly ICarRepository carRepository = new CarRepository();
        private static readonly IAggregateRepository aggregateRepository = new AggregateRepository();
        private readonly ICarService carService = new CarService()
        {
            CarRepository = carRepository,
            WebScraper = new ScraperService(),
            FileRepository = new FileRepository(),
            TrackingRepository = new TrackingRepository()
        };

        // GET: api/Cars
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email).Value;
                var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var cars = await aggregateRepository.GetUserCars(userEmail);
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Cars/5
        [HttpGet("{carId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Get(string carId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                var car = await carRepository.GetCarById(carId);

                if (car.User != userId)
                    throw new BusinessException("Car does not exist");

                return Ok(car);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // POST: api/Cars
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Post([FromBody] CarEntity car)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                car.User = userId;
                var newCar = await carService.InsertCar(car);

                return Ok(newCar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Cars/5
        [HttpPut("{carId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Put(string carId, [FromBody] CarEntity car)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await carService.UpdateCar(userId, carId, car);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{carId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Delete(string carId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await carService.DeleteCar(userId, carId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("/api/allcars")]
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AllCars()
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var cars = await carRepository.GetAllCars();
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("/api/cars/{carId}/images")]
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> InsertImages(string carId, [FromBody] List<string> images)
        {
            try
            {
                await carService.InsertImages(carId, images);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("/api/cars/{carId}/images")]
        [HttpPut]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UpdateImages(string carId, [FromBody] Images images)
        {
            try
            {
                await carService.UpdateImages(carId, images);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("/api/cars/{carId}/equipment")]
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> AddEquipment(string carId, [FromBody] List<Equipment> data)
        {
            try
            {
                await carService.InsertEquipment(carId, data);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
