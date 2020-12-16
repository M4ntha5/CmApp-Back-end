using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface IModelRepository
    {
        Task<List<Model>> GetModels(int makeId);
        Task<Model> GetModel(int makeId, int modelId);
        Task InsertModel(int makeId, NameDTO model);
        Task DeleteModel(int modelId);
        Task UpdateModel(int modelId, NameDTO newModel);

    }
}
