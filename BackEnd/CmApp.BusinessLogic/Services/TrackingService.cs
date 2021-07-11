using CmApp.Contracts;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly ITrackingRepository _trackingRepository;
        private readonly ICarRepository _carRepository;
        private readonly IScraperService _scraperService;

        public TrackingService(ITrackingRepository trackingRepository, ICarRepository carRepository, 
            IScraperService scraperService)
        {
            _trackingRepository = trackingRepository;
            _carRepository = carRepository;
            _scraperService = scraperService;
        }

        public async Task UpdateTracking(int carId, Tracking tracking)
        {
           // tracking.Car = carId;
            var oldTracking = await _trackingRepository.GetTrackingByCar(carId);
            await _trackingRepository.UpdateCarTracking(oldTracking.Id, tracking);
        }

        public async Task<Tracking> LookForTrackingData(int carId)
        {
            var car = _carRepository.GetCarById(carId);
            var tracking = await _trackingRepository.GetTrackingByCar(carId);
            if (tracking == null)
                tracking = await _trackingRepository.InsertTracking(new Tracking { }); //Car = carId });

            var updatedTracking = await _scraperService.TrackingScraper(car, tracking.Id);
            return updatedTracking;
        }
        public async Task<List<string>> LookForTrackingImages(int carId)
        {
            var car = _carRepository.GetCarById(carId);
            var tracking = await _trackingRepository.GetTrackingByCar(carId);
            if (tracking == null)
                throw new BusinessException("Tracking images for this car not found. Try again later");

            var trackingImages = await _scraperService.GetTrackingImagesUrls(car);
            //inserts atlantic image urls
            await _trackingRepository.UploadImageToTracking(tracking.Id, trackingImages);
            return trackingImages;
        }

        public async Task SaveLastShowImagesStatus(int carId, bool status)
        {
            var tracking = await _trackingRepository.GetTrackingByCar(carId);
            if (tracking == null)
                throw new BusinessException("Tracking images for this car not found. Try again later");

            await _trackingRepository.UpdateImageShowStatus(tracking.Id, status);
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
