using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Models;
using CmApp.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Repositories
{
    public class MakeRepository : IMakeRepository
    {
        /* private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
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

             await repo.UpdateOneAsync(make.ID, update, new DatabaseUpdateOneOptions());
         }
         public async Task DeleteCarMake(string makeId)
         {
             var repo = new CodeMashRepository<CarMakesEntity>(Client);

             await repo.DeleteOneAsync(makeId);
         }

         private async Task<CarMakesEntity> CheckForDuplicates(CarMakesEntity make)
         {
             make.Make = make.Make.First().ToString().ToUpper() + make.Make.Substring(1);
             make.Models.ForEach(x => x.Name = x.Name.First().ToString().ToUpper() + x.Name.Substring(1));

             var allMakes = await GetAllMakes();
             foreach (var elem in allMakes)
             {
                 if (elem.Make == make.Make && elem.Id != make.Id)
                     throw new BusinessException("Such a make already exists!");
                 if (elem.Models.Count != elem.Models.Distinct().ToList().Count)
                     throw new BusinessException("You cannot add duplicate models!");
             }
             return make;
         }*/

        private readonly Context _context;

        public MakeRepository(Context context)
        {
            _context = context;
        }

        //v2
        public Task<List<Make>> GetMakes()
        {
            return _context.Makes.Include(x=>x.Models).ToListAsync();
        }
        public Task<Make> GetMake(int makeId)
        {
            return _context.Makes.Include(x => x.Models).FirstOrDefaultAsync(x => x.Id == makeId);
        }
        public Task InsertMake(NameDTO make)
        {
            if (make == null || string.IsNullOrEmpty(make.Name))
                throw new BusinessException("Make not defined");

            var isInDb = _context.Makes.Any(x => x.Name == make.Name);
            if (isInDb)
                throw new BusinessException("Such make already exists");

            var model = new Make
            {
                Name = make.Name
            };

            _context.Makes.AddAsync(model);
            return _context.SaveChangesAsync();
        }
        public async Task DeleteMake(int makeId)
        {
            var make = await _context.Makes.FirstOrDefaultAsync(x => x.Id == makeId);
            if (make != null)
            {
                _context.Remove(make);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateMake(int makeId, NameDTO newMake)
        {
            if (newMake == null || string.IsNullOrEmpty(newMake.Name))
                throw new BusinessException("Make not defined");

            var make = await _context.Makes.FirstOrDefaultAsync(x => x.Id == makeId);
            if(make != null)
            {
                make.Name = newMake.Name;
            }
            await _context.SaveChangesAsync();
        }



        //v1
        public Task DeleteCarMake(int makeId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Make>> GetAllMakes()
        {
            throw new NotImplementedException();
        }

        public Task<Make> GetMakeModels(string make)
        {
            throw new NotImplementedException();
        }

        public Task<Make> InsertCarMake(Make make)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCarMake(Make make)
        {
            throw new NotImplementedException();
        }
    }
}
