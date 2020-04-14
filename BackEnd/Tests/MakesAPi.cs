using CmApp.Repositories;
using NUnit.Framework;
using System.Threading.Tasks;

namespace VechicleAPI.Tests
{
    class MakesAPi
    {
        VehicleAPI vehicleRepo;
        CarRepository carRepo;
        [SetUp]
        public void Setup()
        {
            carRepo = new CarRepository();
            vehicleRepo = new VehicleAPI();
        }

        [Test]
        public async Task GetAll()
        {
            var make = "BMW";
            var result = await vehicleRepo.GetAllMakeModels(make);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task GetAllMakes()
        {
            var makes = await carRepo.GetAllMakes();
            Assert.IsNotEmpty(makes);
        }
        [Test]
        public async Task BadUrl()
        {
            var makes = await vehicleRepo.GetAllMakeModels("asd/asd/asd/df");
            Assert.IsNull(makes);
        }
    }
}
