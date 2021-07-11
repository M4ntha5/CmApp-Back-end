using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using CmApp.Contracts.DTO;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.WebApi.Controllers
{
    [Authorize(Roles = "user, admin", AuthenticationSchemes = "user, admin")]
    [ApiController]
    public class CustomController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly IExternalAPIService _externalApi;
        private readonly IFileRepository _fileRepository;
        private readonly IMakeRepository _carMakesRepository;

        public CustomController(ICarRepository carRepository, IExternalAPIService externalApi, 
            IFileRepository fileRepository, IMakeRepository carMakesRepository)
        {
            _carRepository = carRepository;
            _externalApi = externalApi;
            _fileRepository = fileRepository;
            _carMakesRepository = carMakesRepository;
        }

      /*  [HttpGet]
        [Route("/api/makes")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> GetAllMakes()
        {
            try
            {
                var makes = await carMakesRepository.GetAllMakes();
                //List<string> namesOnly = makes.Select(x => x.Name).ToList();
                //namesOnly.Sort();
                return Ok(makes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("/api/makes/{makeName}/models")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetAllMakeModels(string makeName)
        {
            try
            {
                var models = await carMakesRepository.GetMakeModels(makeName);
                //var makes = await externalAPI.GetAllMakeModels(makeName);
                if (models == null)
                    throw new BusinessException("Error retrieving models. Please try again later.");
                //models.Models.Sort();
                var modelsNames = models.Models.Select(x => x.Name).ToList();
                modelsNames.Sort();
                return Ok(modelsNames);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

        // GET: api/Custom
        [Route("api/user-car-names")]
        [Authorize(Roles = "user")]
        [HttpGet]
        public async Task<IActionResult> GetCarNames()
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var cars = await _carRepository.GetAllUserCars(userId);
                var result = new List<object>();
                foreach (var car in cars)
                    result.Add(new { value = car.Id, text = car.Make + " " });

                if (result.Count != 0)
                    return Ok(result);
                else
                    throw new Exception("No cars yet");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/get-image")]
        [Authorize(Roles = "user")]
        [HttpPost]
        public async Task<IActionResult> GetImage([FromBody] object image)
        {
            try
            {
                var (fileId, fileType) = _fileRepository.GetFileId(image);

                var fileUrl = await _fileRepository.GetFileUrl(fileId);
                return Ok(fileUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("api/get-image2")]
        [Authorize(Roles = "user")]
        [HttpPost]
        public async Task<IActionResult> GetImage2([FromBody] object image)
        {
            try
            {
                var fileInfo = _fileRepository.GetFileId(image);

                var fileId = fileInfo.Item1;
                var fileType = fileInfo.Item2;

                var stream = await _fileRepository.GetFile(fileId);

                var mem = new MemoryStream();
                await stream.CopyToAsync(mem);

                var bytes = _fileRepository.StreamToByteArray(mem);
                var base64 = _fileRepository.ByteArrayToBase64String(bytes);

                base64 = "data:" + fileType + ";base64," + base64;

                return Ok(base64);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/countries")]
        [Authorize(Roles = "user, admin")]
        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _externalApi.GetAllCountries();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Currency
        [Route("/api/currency")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAvailableCurrencies()
        {
            try
            {
                //all rates names
                var names = await _externalApi.GetAvailableCurrencies();
                return Ok(names);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Currency
        [AllowAnonymous]
        [Route("/api/currency")]
        [HttpPost]
        public async Task<IActionResult> CalculateExchangeResult([FromBody] ExchangeInput input)
        {
            try
            {
                //calculates result here
                var result = await _externalApi.CalculateResult(input);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      /*  [HttpPost]
        [Route("/api/makes")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> InsertMake([FromBody] Make carMake)
        {
            try
            {
                var make = await carMakesRepository.InsertCarMake(carMake);
                return Ok(make);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("/api/makes/{makeId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateMake(int makeId, [FromBody] Make carMakes)
        {
            try
            {
                carMakes.Id = makeId;
                await carMakesRepository.UpdateCarMake(carMakes);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("/api/makes/{makeId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteMake(int makeId)
        {
            try
            {
                await carMakesRepository.DeleteCarMake(makeId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

    }
}
