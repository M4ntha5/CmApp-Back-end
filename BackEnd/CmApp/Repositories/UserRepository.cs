using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Utils;
using CodeMash.Client;
using CodeMash.Repository;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CmApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<UserEntity> InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Cannot insert user in db, because user is empty");
            if(user.Password != user.Password2)
                throw new BusinessException("Passwords do not match!!");


            //password hashing
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashedPass = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: user.Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );

            var entity = new UserEntity
            { 
                Email = user.Email,
                Password = hashedPass,
                Salt = Convert.ToBase64String(salt)
            };

            var repo = new CodeMashRepository<UserEntity>(Client);

            var response = await repo.InsertOneAsync(entity, new DatabaseInsertOneOptions());
            return response;
        }

        public async Task<UserEntity> GetUserById(string carId)
        {
            var repo = new CodeMashRepository<UserEntity>(Client);

            var filter = Builders<UserEntity>.Filter.Eq("_id", BsonObjectId.Create(carId));

            var summary = await repo.FindOneAsync(filter, new DatabaseFindOneOptions());

            return summary;
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
            var repo = new CodeMashRepository<UserEntity>(Client);

            var summary = await repo.FindAsync(x=> !x.Deleted, new DatabaseFindOptions());

            return summary.Items;
        }

        public async Task<List<UserEntity>> GetAllBlockedUsers()
        {
            var repo = new CodeMashRepository<UserEntity>(Client);

            var summary = await repo.FindAsync(x => x.Blocked, new DatabaseFindOptions());

            return summary.Items;
        }
        public async Task<List<UserEntity>> GetAllUnblockedUsers()
        {
            var repo = new CodeMashRepository<UserEntity>(Client);

            var summary = await repo.FindAsync(x => !x.Blocked, new DatabaseFindOptions());

            return summary.Items;
        }
        public async Task BlockUser(string userId)
        {
            var repo = new CodeMashRepository<UserEntity>(Client);

            var updateDefinition = Builders<UserEntity>.Update.Set(x => x.Blocked, true);

            await repo.UpdateOneAsync(x=> x.Id == userId, updateDefinition, new DatabaseUpdateOneOptions());
        }
        public async Task UnblockUser(string userId)
        {
            var repo = new CodeMashRepository<UserEntity>(Client);

            var updateDefinition = Builders<UserEntity>.Update.Set(x => x.Blocked, false);

            await repo.UpdateOneAsync(x => x.Id == userId, updateDefinition, new DatabaseUpdateOneOptions());
        }

        public async Task UpdateUser(UserEntity user)
        {
            var repo = new CodeMashRepository<UserEntity>(Client);

            UpdateDefinition<UserEntity>[] updates =
            {
                Builders<UserEntity>.Update.Set(x => x.Name, user.Name),
                Builders<UserEntity>.Update.Set(x => x.Email, user.Email)
            };

            var atributes = Builders<UserEntity>.Update.Combine(updates);

            await repo.UpdateOneAsync(x => x.Id == user.Id, atributes, new DatabaseUpdateOneOptions());
        }
        public async Task<UserEntity> GetUserByEmail(string email)
        {
            var repo = new CodeMashRepository<UserEntity>(Client);

            var user = await repo.FindOneAsync(x => x.Email == email, new DatabaseFindOneOptions());

            return user;
        }


        /*  public async Task<string> CheckIfEmailAlreadyExist(User user)
          {
              var registerService = new CodeMashRepository<ConsumerEntity>(Client);
              var email = await registerService.FindOneAsync(x => x.Email == user.Email);
              var password = await registerService.FindOneAsync(x => x.Password == user.Password);

              if (email != null)
              {
                  return "email already exist";
              }
              else if (password != null)
              {
                  return "password already exist";
              }
          }*/


    }
}
