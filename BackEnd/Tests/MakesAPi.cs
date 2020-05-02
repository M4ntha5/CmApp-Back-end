using CmApp.Repositories;
using CmApp.Utils;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace VechicleAPITests
{
    class MakesAPi
    {
        VehicleAPI vehicleRepo;
        CarRepository carRepo;
        FileRepository fileRepo;
        [SetUp]
        public void Setup()
        {
            carRepo = new CarRepository();
            vehicleRepo = new VehicleAPI();
            fileRepo = new FileRepository();
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
        [Test]
        public async Task GetFileUrl()
        {
            var makes = await fileRepo.GetFileUrl(Settings.DefaultImage);
            Assert.IsNotNull(makes);
        }
                [Test]
        public async Task Conversions()
        {
            var file =await fileRepo.GetFile(Settings.DefaultImage);

            using MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);
            var bytes = ms.ToArray();
            var base64 = fileRepo.ByteArrayToBase64String(bytes);
            Assert.IsNotNull(base64);
            var byts = fileRepo.Base64ToByteArray(base64);
            Assert.IsNotEmpty(base64);
        }
        [Test]
        public async Task GetFileId()
        {
            var car  =await carRepo.GetCarById("5ea5ad855969c94a542a8127");

            var res = fileRepo.GetFileId(car.Images[0]);

            Assert.NotNull(res);
        }
    }
}
