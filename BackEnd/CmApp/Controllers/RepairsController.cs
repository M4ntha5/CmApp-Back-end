using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{
    [Route("/api/cars/{carId}/repairs")]
    [Authorize(Roles = "user, admin", AuthenticationSchemes = "user, admin")]
    [ApiController]
    public class RepairsController : ControllerBase
    {
        private readonly RepairService repairService = new RepairService
        {
            RepairRepository = new RepairRepository(),
            SummaryRepository = new SummaryRepository()
        };
        private readonly CarRepository carRepo = new CarRepository();

        // GET: api/cars/{carId}/Repairs
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
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Get(string carId, string repairId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId && role != "admin")
                    throw new Exception("Car does not exist");

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
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Post(string carId, [FromBody] List<RepairEntity> repairs)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId && role != "admin")
                    throw new Exception("Car does not exist");

                await repairService.InsertCarRepairs(carId, repairs);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/cars/{carId}/Repairs/5
        [HttpPut("{repairId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Put(string carId, string repairId, [FromBody] RepairEntity repair)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId && role != "admin")
                    throw new Exception("Car does not exist");

                await repairService.UpdateSelectedCarRepair(repairId, carId, repair);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete()]
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

                await repairService.DeleteAllCarRepairs(carId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
