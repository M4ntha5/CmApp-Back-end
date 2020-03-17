using CmApp.Contracts;
using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class TrackingService : ITrackingService
    {
        public ITrackingRepository TrackingRepository { get; set; }
        public ICarRepository CarRepository { get; set; }
        public IFileRepository FileRepository { get; set; }
        public IWebScraper ScraperService { get; set; }

        public async Task DeleteTracking(string carId)
        {
            await TrackingRepository.DeleteCarTracking(carId);
        }
        public async Task UpdateTracking(string carId, TrackingEntity tracking)
        {
            tracking.Car = carId;
            var trackingId = TrackingRepository.GetTrackingByCar(carId).Result.Id;
            await TrackingRepository.UpdateCarTracking(trackingId, tracking);
        }
        public async Task<TrackingEntity> GetTracking(string carId)
        {
            var tracking = await TrackingRepository.GetTrackingByCar(carId);
                
            if (tracking != null)
            {
                //fetching only first image
                var fileInfo = FileRepository.GetFileId(tracking.AuctionImages[0]);

                var fileId = fileInfo.Item1;
                var fileType = fileInfo.Item2;

                var stream = await FileRepository.GetFile(fileId);

                var mem = new MemoryStream();
                stream.CopyTo(mem);

                var bytes = FileRepository.StreamToByteArray(mem);
                string base64 = FileRepository.ByteArrayToBase64String(bytes);

                base64 = "data:" + fileType + ";base64," + base64;

                tracking.Base64images.Add(base64);
            }

            return tracking;
        }
        public async Task<TrackingEntity> InsertTracking(string carId, TrackingEntity tracking)
        {
            tracking.Car = carId;
            var newTracking = await TrackingRepository.InsertTracking(tracking);
            return newTracking;
        }

        public async Task<TrackingEntity> LookForTracking(string carId)
        {
            var car = await CarRepository.GetCarById(carId);
            var tracking = await ScraperService.TrackingScraper(car.Vin);
            return tracking;
        }
    }
}
