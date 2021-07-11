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
        private readonly Context _context;

        public MakeRepository(Context context)
        {
            _context = context;
        }

        //v2
        public Task<List<Make>> GetMakes()
        {
            return _context.Makes.OrderBy(x => x.Name).ToListAsync();
        }
        public Task<Make> GetMake(int makeId)
        {
            return _context.Makes.FirstOrDefaultAsync(x => x.Id == makeId);
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
    }
}
