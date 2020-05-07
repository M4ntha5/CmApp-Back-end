using CmApp.Contracts;
using CmApp.Domains;
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
    [Route("/api/cars/{carId}/summary")]
    [Authorize(Roles = "user", AuthenticationSchemes = "user")]
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private static readonly ISummaryRepository summaryRepository = new SummaryRepository();
        private static readonly ICarRepository carRepo = new CarRepository();
        private readonly IAggregateRepository aggRepo = new AggregateRepository();
        private readonly ICarService carService = new CarService()
        {
            SummaryRepository = summaryRepository,
            ExternalAPIService = new ExternalAPIService(),
            CarRepository = carRepo,
            FileRepository = new FileRepository(),
            ShippingRepository = new ShippingRepository(),
            TrackingRepository = new TrackingRepository(),
            WebScraper = new ScraperService()
        };


        // GET: /api/cars/{carId}/summary
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

                var summary = await summaryRepository.GetSummaryByCarId(carId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: /api/cars/{carId}/summary
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Post(string carId, [FromBody] SummaryEntity summary)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId)
                    throw new Exception("Car does not exist");

                var newSummary = await carService.InsertCarSummary(carId, summary);
                return Ok(newSummary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: /api/cars/{carId}/summary/{id}
        [HttpPut]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Put(string carId, [FromBody] SummaryEntity summary)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepo.GetCarById(carId);
                if (car.User != userId)
                    throw new Exception("Car does not exist");

                await carService.UpdateSoldSummary(carId, summary);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: /api/cars/{carId}/summary/{id}
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

                await summaryRepository.DeleteCarSummary(carId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("/api/users/{userId}/stats")]
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetUserSaleStats(string userId, [FromBody] StatsHelper inputData)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (authUserId != userId)
                    throw new Exception("You cannot access this resource");

                var stats = await aggRepo.GetCarStats(inputData.DateFrom, inputData.DateTo, userEmail);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
