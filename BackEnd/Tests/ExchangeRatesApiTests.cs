using CmApp.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates
{
    class ExchangeRatesApiTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAll()
        {
            var repo = new ExchangeRatesRepository();

            var result = await repo.GetLatestForeignExchanges();

            Assert.AreNotEqual(0, result.Rates.Count);
        }

        [Test]
        public async Task GetSelected()
        {
            var repo = new ExchangeRatesRepository();

            var result = await repo.GetSelectedExchangeRate("USD");

            Assert.AreEqual(1, result.Rates.Count);
        }
    }
}
