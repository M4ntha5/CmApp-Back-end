using CmApp.Contracts.Entities;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CmApp.Controllers
{
    [Route("/api/cars/{carId}/repairs")]
    [Authorize(Roles = "user", AuthenticationSchemes = "user")]
    [ApiController]
    public class RepairsController : ControllerBase
    {
        private readonly IRepairRepository repairRepository;
        private readonly ICarRepository carRepository;
        private readonly IRepairService repairService;

        public RepairsController(IRepairRepository repairRepository, ICarRepository carRepository, 
            IRepairService repairService)
        {
            this.repairRepository = repairRepository;
            this.carRepository = carRepository;
            this.repairService = repairService;
        }


        // GET: api/cars/{carId}/Repairs
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Get(int carId)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
               /* if (car.User != userId)
                    throw new Exception("Car does not exist");*/

                var repairs = await repairService.GetAllSelectedCarRepairs(carId);
                return Ok(repairs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/cars/{carId}/Repairs/5
        [HttpGet("{repairId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Get(int carId, int repairId)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
               /* if (car.User != userId)
                    throw new Exception("Car does not exist");*/

                var repair = await repairService.GetSelectedCarRepairById(carId, repairId);
                return Ok(repair);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/cars/{carId}/Repairs
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Post(int carId, [FromBody] List<RepairEntity> repairs)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var car = await carRepository.GetCarById(carId);
                /*if (car.User != userId)
                    throw new Exception("Car does not exist");*/

                await repairService.InsertCarRepairs(carId, repairs);
                return Ok("Successfully inserted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/cars/{carId}/Repairs/5
        [HttpPut("{repairId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Put(int carId, int repairId, [FromBody] RepairEntity repair)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
                /*if (car.User != userId)
                    throw new Exception("Car does not exist");*/

                repair.Car = carId;
                await repairRepository.UpdateRepair(repairId, repair);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        //[Route("api/cars/{carId}/repairs")]
        [HttpDelete]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Delete(int carId)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
              /*  if (car.User != userId)
                    throw new Exception("Car does not exist");*/

                await repairService.DeleteMultipleRepairs(carId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
