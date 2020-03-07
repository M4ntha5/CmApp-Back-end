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
            {
                throw new ArgumentNullException(nameof(shipping), "Cannot insert shipping in db, because shipping is empty");
            }

            var repo = new CodeMashRepository<ShippingEntity>(Client);

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
            shipping.Id = shippingId;

            await repo.ReplaceOneAsync(
                x => x.Id == shipping.Id,
                shipping,
                new DatabaseReplaceOneOptions()
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
