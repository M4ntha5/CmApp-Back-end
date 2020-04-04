using CmApp.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VechicleAPI.Tests
{
    class MakesAPi
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task GetAll()
        {
            var repo = new VehicleAPI();
            var make = "BMW";
            var result = await repo.GetAllMakeModels(make);

        }

        [Test]
        public async Task GetAllMakes()
        {
            var repo = new CarRepository();
            var makes = await repo.GetAllMakes();
        }
    }
}
