using CmApp;
using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Repositories;
using CmApp.Utils;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace ExchangeRatesTests
{
    class ExternalApiTests
    {
        IExternalAPIService APIservice;
        IFileRepository fileRepo;
        [SetUp]
        public void Setup()
        {
            APIservice = new ExternalAPIService();
            fileRepo = new FileRepository();
        }

        [Test]
        public async Task GetAllCurrencies()
        {
            var result = await APIservice.GetAvailableCurrencies();
            Assert.AreNotEqual(0, result.Count);
        }

        [Test]
        public async Task GetAllCountries()
        {
            var curr = await APIservice.GetAllCountries();
            Assert.NotNull(curr);
        }
        [Test]
        public async Task GetSelectedExchangeRate()
        {
            var curr = await APIservice.GetSelectedExchangeRate("EUR");
            Assert.IsNotNull(curr);
        }
        [Test]
        public async Task GetSelectedExchangeRateBad()
        {
            var curr = await APIservice.GetSelectedExchangeRate("EURasdasd");
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
            var curr = await APIservice.CalculateResult(input);
            Assert.NotZero(curr);
            Assert.ThrowsAsync<BusinessException>(async () => await APIservice.CalculateResult(null));
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
            var file = await fileRepo.GetFile(Settings.DefaultImage);

            using MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);
            var bytes = ms.ToArray();
            var base64 = fileRepo.ByteArrayToBase64String(bytes);
            Assert.IsNotNull(base64);
            var byts = fileRepo.Base64ToByteArray(base64);
            Assert.IsNotEmpty(base64);
        }
    }
}
