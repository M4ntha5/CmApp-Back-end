using CmApp.Contracts;
using CmApp.Contracts.Entities;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly ITrackingRepository TrackingRepository;
        private readonly ICarRepository CarRepository;
        private readonly IScraperService ScraperService;

        public TrackingService(ITrackingRepository trackingRepository, ICarRepository carRepository, 
            IScraperService scraperService)
        {
            TrackingRepository = trackingRepository;
            CarRepository = carRepository;
            ScraperService = scraperService;
        }

        public async Task UpdateTracking(int carId, TrackingEntity tracking)
        {
           // tracking.Car = carId;
            var oldTracking = await TrackingRepository.GetTrackingByCar(carId);
            await TrackingRepository.UpdateCarTracking(oldTracking.ID, tracking);
        }

        public async Task<TrackingEntity> LookForTrackingData(int carId)
        {
            var car = await CarRepository.GetCarById(carId);
            var tracking = await TrackingRepository.GetTrackingByCar(carId);
            if (tracking == null)
                tracking = await TrackingRepository.InsertTracking(new TrackingEntity { }); //Car = carId });

            var updatedTracking = await ScraperService.TrackingScraper(car, tracking.ID);
            return updatedTracking;
        }
        public async Task<List<string>> LookForTrackingImages(int carId)
        {
            var car = await CarRepository.GetCarById(carId);
            var tracking = await TrackingRepository.GetTrackingByCar(carId);
            if (tracking == null)
                throw new BusinessException("Tracking images for this car not found. Try again later");

            var trackingImages = await ScraperService.GetTrackingImagesUrls(car);
            //inserts atlantic image urls
            await TrackingRepository.UploadImageToTracking(tracking.ID, trackingImages);
            return trackingImages;
        }

        public async Task SaveLastShowImagesStatus(int carId, bool status)
        {
            var tracking = await TrackingRepository.GetTrackingByCar(carId);
            if (tracking == null)
                throw new BusinessException("Tracking images for this car not found. Try again later");

            await TrackingRepository.UpdateImageShowStatus(tracking.ID, status);
        }

        //bring back if needed
        /*     
        public async Task DownloadTrackingImages(string carId, List<string> urls)
        {
            var tracking = await TrackingRepository.GetTrackingByCar(carId);
            if (tracking == null)
                throw new BusinessException("Tracking images for this car not found. Try again later");

            await ScraperService.DownloadAllTrackingImages(tracking, urls);
        }
        */

    }
}
