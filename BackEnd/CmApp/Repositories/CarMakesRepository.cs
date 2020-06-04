using CmApp.Contracts;
using CmApp.Entities;
using CmApp.Utils;
using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Repositories
{
    public class CarMakesRepository : ICarMakesRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
        public async Task<List<CarMakesEntity>> GetAllMakes()
        {
            var service = new CodeMashRepository<CarMakesEntity>(Client);

            var makes = await service.FindAsync(
                x => true,
                new DatabaseFindOptions()
            );
            return makes.Items;
        }

        public async Task<CarMakesEntity> InsertCarMake(CarMakesEntity make)
        {           
            if (make == null)
                throw new ArgumentNullException(nameof(make), "Cannot insert make in db, because make is empty");
            
            var repo = new CodeMashRepository<CarMakesEntity>(Client);

            var makes = await repo.InsertOneAsync(make, new DatabaseInsertOneOptions());
            return makes;
        }

        public async Task UpdateCarMake(CarMakesEntity make)
        {           
            var repo = new CodeMashRepository<CarMakesEntity>(Client);

            var update = Builders<CarMakesEntity>.Update
                .Set("name", make.Name)
                .Set("models", make.Models);

            await repo.UpdateOneAsync(make.Id, update, new DatabaseUpdateOneOptions());
        }
        public async Task DeleteCarMake(string makeId)
        {           
            var repo = new CodeMashRepository<CarMakesEntity>(Client);

            await repo.DeleteOneAsync(makeId);
        }
    }
}
