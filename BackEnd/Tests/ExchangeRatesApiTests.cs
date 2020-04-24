using CmApp.Domains;
using CmApp.Repositories;
using CmApp.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates
{
    class ExchangeRatesApiTests
    {
        ExchangeService repo;
        [SetUp]
        public void Setup()
        {
            repo = new ExchangeService();
        }

        [Test]
        public async Task GetAll()
        {
            var result = await repo.GetAvailableCurrencies();
            Assert.AreNotEqual(0, result.Count);
        }

        [Test]
        public async Task GetAllCountries()
        {
            var curr = await repo.GetAllCountries();
            Assert.NotNull(curr);
        }
        [Test]
        public async Task GetSelectedExchangeRate()
        {
            var curr = await repo.GetSelectedExchangeRate("EUR");
            Assert.IsNotNull(curr);
        }
        [Test]
        public async Task GetSelectedExchangeRateBad()
        {
            var curr = await repo.GetSelectedExchangeRate("EURasdasd");
            Assert.IsNull(curr);
        }
        [Test]
        public async Task CalculateResult()
        {
            var input = new ExchangeInput
            {
                Amount = 10,
                From = "USD",
                To = "EUR"
            };
            var curr = await repo.CalculateResult(input);
            Assert.NotZero(curr);
        }
    }
}
