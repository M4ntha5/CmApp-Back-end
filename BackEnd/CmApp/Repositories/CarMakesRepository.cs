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

            var sort = Builders<CarMakesEntity>.Sort.Ascending("make");

            var makes = await service.FindAsync<CarMakesEntity>(
                x => true,
                null,
                sort,
                new DatabaseFindOptions()
            );
            return makes.Items;
        }

        public async Task<CarMakesEntity> GetMakeModels(string make)
        {
            var service = new CodeMashRepository<CarMakesEntity>(Client);

            var makeModels = await service.FindOneAsync(
                x => x.Make == make,
                new DatabaseFindOneOptions()
            );
            return makeModels;
        }

        public async Task<CarMakesEntity> InsertCarMake(CarMakesEntity make)
        {           
            if (make == null)
                throw new ArgumentNullException(nameof(make), "Cannot insert make in db, because make is empty");
            
            var repo = new CodeMashRepository<CarMakesEntity>(Client);

            make = await CheckForDuplicates(make);

            var newMake = await repo.InsertOneAsync(make, new DatabaseInsertOneOptions());
            return newMake;
        }

        public async Task UpdateCarMake(CarMakesEntity make)
        {           
            var repo = new CodeMashRepository<CarMakesEntity>(Client);          

            make = await CheckForDuplicates(make);

            var update = Builders<CarMakesEntity>.Update
                .Set("make", make.Make)
                .Set("models", make.Models);

            await repo.UpdateOneAsync(make.Id, update, new DatabaseUpdateOneOptions());
        }
        public async Task DeleteCarMake(string makeId)
        {           
            var repo = new CodeMashRepository<CarMakesEntity>(Client);

            await repo.DeleteOneAsync(makeId);
        }

        private async Task<CarMakesEntity> CheckForDuplicates(CarMakesEntity make)
        {
            make.Make = make.Make.First().ToString().ToUpper() + make.Make.Substring(1);
            make.Models.ForEach(x=> x.Name = x.Name.First().ToString().ToUpper() + x.Name.Substring(1));
                      
            var allMakes = await GetAllMakes();            
            foreach(var elem in allMakes)
            {
                if(elem.Make == make.Make && elem.Id != make.Id)
                    throw new BusinessException("Such a make already exists!");
                if(elem.Models.Count != elem.Models.Distinct().ToList().Count)
                    throw new BusinessException("You cannot add duplicate models!");
            }
            return make;
        }
    }
}
