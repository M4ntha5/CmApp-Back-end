using CmApp.Entities;
using CmApp.Utils;
using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Repositories
{
    public class PasswordResetRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task InsertPasswordReset(PasswordResetEntity resetEntity)
        {
            var repo = new CodeMashRepository<PasswordResetEntity>(Client);

            await repo.InsertOneAsync(resetEntity, new DatabaseInsertOneOptions());
        }

        public async Task<PasswordResetEntity> GetPasswordResetByToken(string token)
        {
            var repo = new CodeMashRepository<PasswordResetEntity>(Client);

            var filter = Builders<PasswordResetEntity>.Filter.Eq("token", token);

            var resetDetails = await repo.FindOneAsync(filter, new DatabaseFindOneOptions());
            return resetDetails;
        }


    }
}
