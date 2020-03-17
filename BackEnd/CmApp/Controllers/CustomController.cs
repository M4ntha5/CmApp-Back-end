using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CmApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{

    [ApiController]
    public class CustomController : ControllerBase
    {
        CarRepository repo = new CarRepository();

        // GET: api/Custom
        [Route("api/user-car-names")]
        [HttpGet]
        public async Task<List<object>> GetCarNames()
        {
            var cars = await repo.GetAllCars();
            var result = new List<object>();

            foreach(var car in cars)
                result.Add( new { id = car.Id, name = car.Make + " " + car.Model });

            if (result.Count != 0)
                return result;
            else
                throw new Exception("No cars yet");
        }

        [Route("api/get-images")]
        [HttpPost]
        public async Task<List<string>> GetImages([FromBody] List<object> images)
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
            return imgs;
        }
    }
}
