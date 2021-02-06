using CmApp.Contracts;
using CmApp.Contracts.Models;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CmApp.Contracts.DTO;
using CmApp.Contracts.DTO.v2;

namespace CmApp.Controllers
{
    [Route("/api/cars")]
    [Authorize(Roles = "user, admin", AuthenticationSchemes = "user, admin")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly ICarService _carService;

        public CarsController(ICarRepository carRepository, ICarService carService)
        {
            _carRepository = carRepository;
            _carService = carService;
        }


        // GET: api/Cars
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var cars = await _carService.GetUserCars(userId);
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
        public async Task<IActionResult> Get(int carId)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                var car = await _carRepository.GetCarById(carId);

                //if (car.User != userId)
                //    throw new BusinessException("Car does not exist");

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
        public async Task<IActionResult> Post([FromBody] CarDTO car)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
               // car.User = userId;
                var newCar = await _carService.InsertCar(userId, car);

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
        public async Task<IActionResult> Put(int carId, [FromBody] Car car)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _carService.UpdateCar(userId, carId, car);
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
        public async Task<IActionResult> Delete(int carId)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _carService.DeleteCar(userId, carId);
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
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var cars = await _carRepository.GetAllCars();
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
        public async Task<IActionResult> InsertImages(int carId, [FromBody] List<string> images)
        {
            try
            {
                await _carService.InsertImages(carId, images);
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
        public async Task<IActionResult> UpdateImages(int carId, [FromBody] Images images)
        {
            try
            {
                await _carService.UpdateImages(carId, images);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
