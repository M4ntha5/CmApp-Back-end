using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LambdaFunction
{
    public class Repo
    {
        private static CodeMashClient Client => new CodeMashClient(CodeMashSettings.ApiKey, CodeMashSettings.ProjectId);
        
        public async Task<List<UserEntity>> GetAllUsers()
        {
            var repo = new CodeMashRepository<UserEntity>(Client);
            var users = await repo.FindAsync(x => true, new DatabaseFindOptions());
            return users.Items;
        }
        
        //cars
        public async Task<List<CarEntity>> GetAllCars()
        {
            var repo = new CodeMashRepository<CarEntity>(Client);
            var cars = await repo.FindAsync(x => true, new DatabaseFindOptions());
            return cars.Items;
        }
        public async Task DeleteCar(string carId)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);
            await repo.DeleteOneAsync(x => x.Id == carId);
        }
        //tracking
        public async Task<List<TrackingEntity>> GetAllTrackings()
        {
            var repo = new CodeMashRepository<TrackingEntity>(Client);

            var trackings = await repo.FindAsync(x => true, new DatabaseFindOptions());
            return trackings.Items;
        }
        public async Task DeleteCarTracking(string carId)
        {
            var repo = new CodeMashRepository<TrackingEntity>(Client);
            var filter = Builders<TrackingEntity>.Filter.Eq("car", BsonObjectId.Create(carId));
            await repo.DeleteOneAsync(filter);
        }
        //summary
        public async Task<List<SummaryEntity>> GetAllSummaries()
        {
            var repo = new CodeMashRepository<SummaryEntity>(Client);
            var summary = await repo.FindAsync(x => true, new DatabaseFindOptions());
            return summary.Items;
        }
        public async Task DeleteCarSummary(string carId)
        {
            var repo = new CodeMashRepository<SummaryEntity>(Client); 
            var filter = Builders<SummaryEntity>.Filter.Eq("car", BsonObjectId.Create(carId));
            await repo.DeleteOneAsync(filter);
        }
        //shipping
        public async Task<List<ShippingEntity>> GetAllShippings()
        {
            var repo = new CodeMashRepository<ShippingEntity>(Client);
            var response = await repo.FindAsync(x => true, new DatabaseFindOptions());
            return response.Items;
        }
        public async Task DeleteCarShipping(string carId)
        {
            var repo = new CodeMashRepository<ShippingEntity>(Client);
            var filter = Builders<ShippingEntity>.Filter.Eq("car", BsonObjectId.Create(carId));
            await repo.DeleteOneAsync(filter);
        }
        //repairs
        public async Task<List<RepairEntity>> GetAllRepairs()
        {
            var repo = new CodeMashRepository<RepairEntity>(Client);
            var repairs = await repo.FindAsync(x => true, new DatabaseFindOptions());
            return repairs.Items;
        }
        public async Task DeleteRepair(string carId)
        {
            var repo = new CodeMashRepository<RepairEntity>(Client);
            var filter = Builders<RepairEntity>.Filter.Eq("car", ObjectId.Parse(carId));
            await repo.DeleteOneAsync(filter);
        }
        //pass resets
        public async Task<List<PasswordResetEntity>> GetAllPasswordResets()
        {
            var repo = new CodeMashRepository<PasswordResetEntity>(Client);
            var resetDetails = await repo.FindAsync(x=>true, new DatabaseFindOptions());
            return resetDetails.Items;
        }
        public async Task DeletePassReset(string resetId)
        {
            var repo = new CodeMashRepository<PasswordResetEntity>(Client);
            var filter = Builders<PasswordResetEntity>.Filter.Eq("_id", ObjectId.Parse(resetId));
            await repo.DeleteOneAsync(filter);
        }
    }
}
