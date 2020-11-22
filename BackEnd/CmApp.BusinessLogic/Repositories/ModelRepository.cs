using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Entities;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Repositories
{
    public class ModelRepository : IModelRepository
    {
        private readonly Context _context;

        public ModelRepository(Context context)
        {
            _context = context;
        }
        public Task<List<Model>> GetModels(int makeId)
        {
            return _context.Models.Where(x => x.MakeId == makeId).ToListAsync();
        }
        public Task<Model> GetModel(int makeId, int modelId)
        {
            return _context.Models.FirstOrDefaultAsync(x => x.MakeId == makeId && x.Id == modelId);
        }
        public Task InsertModel(int makeId, NameDTO model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
                throw new BusinessException("Model not defined");

            var isInDb = _context.Models.Where(x => x.Id == makeId).Any(x => x.Name == model.Name);
            if (isInDb)
                throw new BusinessException("Such model already exists");

            var entity = new Model
            {
                Name = model.Name,
                MakeId = makeId
            };
            _context.Models.AddAsync(entity);
            return _context.SaveChangesAsync();
        }
        public async Task DeleteModel(int modelId)
        {
            var model = await _context.Models.FirstOrDefaultAsync(x => x.Id == modelId);
            if (model != null)
            {
                _context.Remove(model);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateModel(int modelId, NameDTO newModel)
        {
            if (newModel == null || string.IsNullOrEmpty(newModel.Name))
                throw new BusinessException("Model not defined");

            var make = await _context.Models.FirstOrDefaultAsync(x => x.Id == modelId);
            if (make != null)
            {
                make.Name = newModel.Name;
            }
            await _context.SaveChangesAsync();
        }
    }
}
