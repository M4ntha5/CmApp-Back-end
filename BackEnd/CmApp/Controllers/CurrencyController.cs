using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmApp.Domains;
using CmApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{
    [Route("api/currency")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ExchangeService repo = new ExchangeService();

        // GET: api/Currency
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                //all rates names
                var names = await repo.GetAvailableCurrencies();
                return Ok(names);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Currency
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExchangeInput input)
        {
            try
            {
                //calculates result here
                var result = await repo.CalculateResult(input);
                return Ok(result);
            }          
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
