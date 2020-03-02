﻿using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IShippingRepository
    {
        Task<ShippingEntity> InsertShipping(ShippingEntity shipping);
        Task DeleteCarShipping(string carId);
        Task UpdateCarShipping(string shippingId, ShippingEntity shipping);
        Task<ShippingEntity> GetShippingByCar(string carId);

    }
}
