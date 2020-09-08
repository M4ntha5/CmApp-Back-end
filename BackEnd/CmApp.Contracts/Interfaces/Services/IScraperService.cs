using CmApp.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface IScraperService
    {
        Dictionary<string, string> GetVehicleInfo(string vin, string make);
        Dictionary<string, string> GetVehicleEquipment(string vin, string make);
        Task<TrackingEntity> TrackingScraper(CarEntity car, int trackingId);
        Task<List<string>> GetTrackingImagesUrls(CarEntity car);
        //bring back if needed
        //Task DownloadAllTrackingImages(TrackingEntity tracking, List<string> imageUrls);
    }
}
