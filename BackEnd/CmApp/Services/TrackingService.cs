using CmApp.Contracts;
using CmApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class TrackingService : ITrackingService
    {
        public ITrackingRepository TrackingRepository { get; set; }
        public ICarRepository CarRepository { get; set; }
        public IScraperService ScraperService { get; set; }

        public async Task UpdateTracking(string carId, TrackingEntity tracking)
        {
            tracking.Car = carId;
            var oldTracking = await TrackingRepository.GetTrackingByCar(carId);
            await TrackingRepository.UpdateCarTracking(oldTracking.Id, tracking);
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

        public async Task SaveLastShowImagesStatus(string carId, bool status)
        {
            var tracking = await TrackingRepository.GetTrackingByCar(carId);
            if (tracking == null)
                throw new BusinessException("Tracking images for this car not found. Try again later");

            await TrackingRepository.UpdateImageShowStatus(tracking.Id, status);
        }
    }
}
