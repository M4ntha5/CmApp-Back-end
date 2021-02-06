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
        /*  private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

          public async Task<ShippingEntity> InsertShipping(ShippingEntity shipping)
          {
              if (shipping == null)
                  throw new ArgumentNullException(nameof(shipping), "Cannot insert shipping in db, because shipping is empty");

              var repo = new CodeMashRepository<ShippingEntity>(Client);

              shipping.AuctionFee = Math.Round(shipping.AuctionFee, 2);
              shipping.Customs = Math.Round(shipping.Customs, 2);
              shipping.TransferFee = Math.Round(shipping.TransferFee, 2);
              shipping.TransportationFee = Math.Round(shipping.TransportationFee, 2);

              var response = await repo.InsertOneAsync(shipping, new DatabaseInsertOneOptions());
              return response;
          }

          public async Task DeleteCarShipping(string carId)
          {
              var repo = new CodeMashRepository<ShippingEntity>(Client);
              var filter = Builders<ShippingEntity>.Filter.Eq("car", BsonObjectId.Create(carId));
              await repo.DeleteOneAsync(filter);
          }

          public async Task UpdateCarShipping(string shippingId, ShippingEntity shipping)
          {
              var repo = new CodeMashRepository<ShippingEntity>(Client);

              var update = Builders<ShippingEntity>.Update
                  .Set("auction_fee", Math.Round(shipping.AuctionFee, 2))
                  .Set("customs", Math.Round(shipping.Customs, 2))
                  .Set("transfer_fee", Math.Round(shipping.TransferFee, 2))
                  .Set("transportation_fee", Math.Round(shipping.TransportationFee, 2));

              await repo.UpdateOneAsync(
                   shippingId,
                   update,
                   new DatabaseUpdateOneOptions()
               );
          }
          public async Task<ShippingEntity> GetShippingByCar(string carId)
          {
              var repo = new CodeMashRepository<ShippingEntity>(Client);
              var filter = Builders<ShippingEntity>.Filter.Eq("car", BsonObjectId.Create(carId));
              var response = await repo.FindOneAsync(filter, new DatabaseFindOneOptions());
              return response;
          }*/

        private readonly Context _context;

        public ShippingRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteCarShipping(int carId)
        {
            /*var shipping = await _context.Shippings.FirstOrDefaultAsync(x => x.CarId == carId);
            if (shipping != null)
            {
                _context.Shippings.Remove(shipping);
                await _context.SaveChangesAsync();
            }*/
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
        }
    }
}