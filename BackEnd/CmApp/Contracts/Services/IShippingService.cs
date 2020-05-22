using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IShippingService
    { 
        Task UpdateShipping(string carId, ShippingEntity shipping);
        Task<ShippingEntity> InsertShipping(string carId, ShippingEntity shipping);
    }
}
