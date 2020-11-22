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
    [Route("api/makes/{makeId}/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase
    {
        private readonly IModelRepository _modelRepo;

        public ModelsController(IModelRepository modelRepo)
        {
            _modelRepo = modelRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int makeId)
        {
            try
            {
                var makes = await _modelRepo.GetModels(makeId);
                return Ok(makes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{modelId}")]
        public async Task<IActionResult> Get(int makeId, int modelId)
        {
            try
            {
                var make = await _modelRepo.GetModel(makeId, modelId);
                return Ok(make);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(int makeId, [FromForm] NameDTO model)
        {
            try
            {
                await _modelRepo.InsertModel(makeId, model);
                return CreatedAtAction(nameof(Post), model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{modelId}")]
        public async Task<IActionResult> Put(int modelId, [FromForm] NameDTO make)
        {
            try
            {
                await _modelRepo.UpdateModel(modelId, make);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{modelId}")]
        public async Task<IActionResult> Delete(int modelId)
        {
            try
            {
                await _modelRepo.DeleteModel(modelId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
