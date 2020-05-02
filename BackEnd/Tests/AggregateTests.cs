using CmApp.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AggregateTests
{
    class AggregateTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task AggregateCarStats()
        {
            var repo = new AggregateRepository();

            var from = new DateTime(2020,04,01,0,0,0,DateTimeKind.Utc);
            var to = new DateTime(2020,06,01,0,0,0,DateTimeKind.Utc);
            var user = "mantozerix@gmail.com";

            var stats = await repo.GetCarStats(from, to, user);
            Assert.IsNotNull(stats);
        }
    }
}
