using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IWebScraper
    {
        Dictionary<string, string> GetVehicleInfo(string vin, string make);
        Dictionary<string, string> GetVehicleEquipment(string vin, string make);
        Task TrackingScraper(string vin);
    }
}
