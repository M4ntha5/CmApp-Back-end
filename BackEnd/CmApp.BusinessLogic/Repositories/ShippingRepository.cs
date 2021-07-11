using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Models;
using CmApp.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Repositories
{
    public class ShippingRepository : IShippingRepository
    {
       /* private readonly Context _context;

        public ShippingRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteCarShipping(int carId)
        {
            var shipping = await _context.Shippings.FirstOrDefaultAsync(x => x.CarId == carId);
            if (shipping != null)
            {
                _context.Shippings.Remove(shipping);
                await _context.SaveChangesAsync();
            }
        }

        public Task<Shipping> GetShippingByCar(int carId)
        {
            return null;// return _context.Shippings.FirstOrDefaultAsync(x => x.CarId == carId);
        }

        public async Task<Shipping> InsertShipping(Shipping shipping)
        {
            if (shipping == null)
                throw new ArgumentNullException(nameof(shipping), "Cannot insert shipping in db, because shipping is empty");

            await _context.Shippings.AddAsync(shipping);
            await _context.SaveChangesAsync();
            return shipping;
        }

        public async Task UpdateCarShipping(int shippingId, Shipping newShipping)
        {
            var shipping = await _context.Shippings.FirstOrDefaultAsync(x => x.Id == shippingId);
            if (shipping != null)
            {
                shipping.AuctionFee = newShipping.AuctionFee;
                shipping.Customs = newShipping.Customs;
                shipping.TransferFee = newShipping.TransferFee;
                shipping.TransportationFee = newShipping.TransportationFee;
                await _context.SaveChangesAsync();
            }
        }*/
    }
}