using CmApp.Contracts;
using CmApp.Entities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class TrackingService : ITrackingService
    {
        public ITrackingRepository TrackingRepository { get; set; }
        public ICarRepository CarRepository { get; set; }
        public IFileRepository FileRepository { get; set; }
        public IScraperService ScraperService { get; set; }

        public async Task DeleteTracking(string carId)
        {
            await TrackingRepository.DeleteCarTracking(carId);
        }
        public async Task UpdateTracking(string carId, TrackingEntity tracking)
        {
            tracking.Car = carId;
            var oldTracking = await TrackingRepository.GetTrackingByCar(carId);
            await TrackingRepository.UpdateCarTracking(oldTracking.Id, tracking);
        }
        public async Task<TrackingEntity> GetTracking(string carId)
        {
            var tracking = await TrackingRepository.GetTrackingByCar(carId);
                
            /*if (tracking != null && tracking.AuctionImages != null && tracking.AuctionImages.Count > 0)
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
            }*/

            return tracking;
        }
        public async Task<TrackingEntity> InsertTracking(string carId, TrackingEntity tracking)
        {
            tracking.Car = carId;
            var newTracking = await TrackingRepository.InsertTracking(tracking);
            return newTracking;
        }

        public async Task<TrackingEntity> LookForTrackingData(string carId)
        {
            var car = await CarRepository.GetCarById(carId);
            var tracking = await TrackingRepository.GetTrackingByCar(carId);
            if (tracking == null)
                tracking = await TrackingRepository.InsertTracking(new TrackingEntity { Car = carId });
           
            var updatedTracking = await ScraperService.TrackingScraper(car, tracking.Id);
            return updatedTracking;
        }
        public async Task<List<string>> LookForTrackingImages(string carId)
        {
            var car = await CarRepository.GetCarById(carId);
            var tracking = await TrackingRepository.GetTrackingByCar(carId);
            if (tracking == null)
                throw new BusinessException("Tracking images for this car not found. Try again later");
            
            var trackingImages = await ScraperService.GetTrackingImagesUrls(car);
            return trackingImages;
        }
        public async Task DownloadTrackingImages(string carId, List<string> urls)
        {
            var tracking = await TrackingRepository.GetTrackingByCar(carId);
            if (tracking == null)
                throw new BusinessException("Tracking images for this car not found. Try again later");

            await ScraperService.DownloadAllTrackingImages(tracking, urls);
        }
    }
}
