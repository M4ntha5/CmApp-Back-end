using CmApp.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface ITrackingService
    {
        Task UpdateTracking(int carId, Tracking tracking);
        Task<Tracking> LookForTrackingData(int carId);
        Task<List<string>> LookForTrackingImages(int carId);
        Task SaveLastShowImagesStatus(int carId, bool status);
        //bring back if needed
        //Task DownloadTrackingImages(string carId, List<string> urls);
    }
}
