using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.WebApi.Controllers
{
    [Route("/api/cars/{carId}/repairs")]
    [Authorize(Roles = "user", AuthenticationSchemes = "user")]
    [ApiController]
    public class RepairsController : ControllerBase
    {
        private readonly IRepairRepository _repairRepository;
        private readonly IRepairService _repairService;

        public RepairsController(IRepairRepository repairRepository, IRepairService repairService)
        {
            _repairRepository = repairRepository;
            _repairService = repairService;
        }


        // GET: api/cars/{carId}/Repairs
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Get(int carId)
        {
            try
            {
                var repairs = await _repairService.GetAllSelectedCarRepairs(carId);
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
                var repair = await _repairService.GetSelectedCarRepairById(carId, repairId);
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
        public async Task<IActionResult> Post(int carId, [FromBody] List<RepairDTO> repairs)
        {
            try
            {
                await _repairService.InsertCarRepairs(carId, repairs);
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
        public async Task<IActionResult> Put(int carId, int repairId, [FromBody] Repair repair)
        {
            try
            {
                await _repairRepository.UpdateRepair(repairId, repair);
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
                await _repairService.DeleteMultipleRepairs(carId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
