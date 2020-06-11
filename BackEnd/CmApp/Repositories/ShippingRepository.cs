using CmApp.Contracts;
using CmApp.Entities;
using CmApp.Utils;
using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace CmApp.Repositories
{
    public class ShippingRepository : IShippingRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

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
        }
    }
}