using CmApp.Contracts.DTO;
using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
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
        private readonly ISummaryRepository summaryRepository;
        private readonly ICarRepository carRepository;
        private readonly ISummaryService summaryService;

        public SummaryController(ISummaryRepository summaryRepository, ICarRepository carRepository, 
            ISummaryService summaryService)
        {
            this.summaryRepository = summaryRepository;
            this.carRepository = carRepository;
            this.summaryService = summaryService;
        }



        // GET: /api/cars/{carId}/summary
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
        public async Task<IActionResult> Post(int carId, [FromBody] SummaryDTO summary)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
               /* if (car.User != userId)
                    throw new Exception("Car does not exist");*/

                await summaryService.InsertCarSummary(carId, summary);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: /api/cars/{carId}/summary/{id}
        [HttpPut]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Put(int carId, [FromBody] Summary summary)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
               /* if (car.User != userId)
                    throw new Exception("Car does not exist");*/

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
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Delete(int carId)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                var car = await carRepository.GetCarById(carId);
               /* if (car.User != userId)
                    throw new Exception("Car does not exist");*/

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
        public async Task<IActionResult> GetUserSaleStats(int userId, [FromBody] StatsHelper inputData)
        {
            try
            {
                var authUserId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (authUserId != userId)
                    throw new Exception("You cannot access this resource");

                var stats = await carRepository.GetCarStats(inputData.DateFrom, inputData.DateTo, userEmail);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
