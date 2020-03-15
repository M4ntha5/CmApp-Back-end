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
        private readonly ExchangeRatesRepository repo = new ExchangeRatesRepository();

        // GET: api/Currency
        [HttpGet]
        public async Task<List<string>> Get()
        {
            //all rates names
            var names = await repo.GetAvailableCurrencies();

            return names;
        }

        // POST: api/Currency
        [HttpPost]
        public async Task<double> Post([FromBody] ExchangeInput input)
        {
            //calculates result here
            var result = await repo.CalculateResult(input);
            return result;
        }
    }
}
