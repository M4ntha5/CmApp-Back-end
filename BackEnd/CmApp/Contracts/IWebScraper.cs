using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IWebScraper
    {
        Dictionary<string, string> GetVehicleInfo(string vin);
        Dictionary<string, string> GetVehicleEquipment(string vin);
    }
}
