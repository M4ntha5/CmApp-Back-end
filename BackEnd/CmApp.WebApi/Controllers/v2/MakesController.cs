using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.WebApi.Controllers.v2
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakesController : ControllerBase
    {
        private readonly IMakeRepository _makeRepo;

        public MakesController(IMakeRepository makeRepo)
        {
            _makeRepo = makeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var makes = await _makeRepo.GetMakes();
                return Ok(makes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{makeId}")]
        public async Task<IActionResult> Get(int makeId)
        {
            try
            {
                var make = await _makeRepo.GetMake(makeId);
                return Ok(make);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] NameDTO make)
        {
            try
            {
                await _makeRepo.InsertMake(make);
                return CreatedAtAction(nameof(Post), make);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{makeId}")]
        public async Task<IActionResult> Put(int makeId, [FromForm] NameDTO make)
        {
            try
            {
                await _makeRepo.UpdateMake(makeId, make);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{makeId}")]
        public async Task<IActionResult> Delete(int makeId)
        {
            try
            {
                await _makeRepo.DeleteMake(makeId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
