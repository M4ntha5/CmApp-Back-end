using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CmApp.Entities;
using CmApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{
    [Authorize(Roles = "user, admin", AuthenticationSchemes = "user, admin")]
    [ApiController]
    public class CustomController : ControllerBase
    {
        readonly CarRepository repo = new CarRepository();

        // GET: api/Cars
        [HttpGet]
        [Route("/api/all-cars")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllCars()
        {
            try
            {
                var cars = await repo.GetAllCars();
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Custom
        [Route("api/user-car-names")]
        [Authorize(Roles = "user, admin")]
        [HttpGet]
        public async Task<IActionResult> GetCarNames()
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var cars = await repo.GetAllUserCars(userId);
                var result = new List<object>();

                foreach (var car in cars)
                    result.Add(new { id = car.Id, name = car.Make + " " + car.Model });

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

        [Route("api/get-images")]
        [Authorize(Roles = "user, admin")]
        [HttpPost]
        public async Task<IActionResult> GetImages([FromBody] List<object> images)
        {
            try
            {
                FileRepository fileRepo = new FileRepository();
                List<string> imgs = new List<string>();
                //images.RemoveAt(0);
                foreach (var image in images)
                {
                    var fileInfo = fileRepo.GetFileId(image);

                    var fileId = fileInfo.Item1;
                    var fileType = fileInfo.Item2;

                    var stream = await fileRepo.GetFile(fileId);

                    var mem = new MemoryStream();
                    stream.CopyTo(mem);

                    var bytes = fileRepo.StreamToByteArray(mem);
                    string base64 = fileRepo.ByteArrayToBase64String(bytes);

                    base64 = "data:" + fileType + ";base64," + base64;

                    imgs.Add(base64);
                }
                return Ok(imgs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
