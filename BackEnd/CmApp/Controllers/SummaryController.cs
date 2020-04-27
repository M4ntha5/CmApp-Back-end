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
    [Route("/api/cars/{carId}/summary")]
    [Authorize(Roles = "user, admin", AuthenticationSchemes = "user, admin")]
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private readonly SummaryService summaryService = new SummaryService()
        {
            SummaryRepository = new SummaryRepository(),
            ExchangeRepository = new ExchangeService(),
        };
        private readonly CarRepository carRepo = new CarRepository();

        // GET: /api/cars/{carId}/summary
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

                var summary = await summaryService.GetSummaryByCarId(carId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: /api/cars/{carId}/summary
        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Post(string carId, [FromBody] SummaryEntity summary)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId && role != "admin")
                    throw new Exception("Car does not exist");

                var newSummary = await summaryService.InsertCarSummary(carId, summary);
                return Ok(newSummary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: /api/cars/{carId}/summary/{id}
        [HttpPut]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Put(string carId, [FromBody] SummaryEntity summary)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId && role != "admin")
                    throw new Exception("Car does not exist");

                await summaryService.UpdateSoldSummary(carId, summary);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: /api/cars/{carId}/summary/{id}
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

                await summaryService.DeleteSummary(carId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
