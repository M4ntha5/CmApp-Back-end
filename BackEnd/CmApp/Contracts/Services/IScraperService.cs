using CmApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IScraperService
    {
        Dictionary<string, string> GetVehicleInfo(string vin, string make);
        Dictionary<string, string> GetVehicleEquipment(string vin, string make);
        Task<TrackingEntity> TrackingScraper(CarEntity car, string trackingId);
        Task<List<string>> DownloadAllTrackingImages(CarEntity car, TrackingEntity tracking);
    }
}
