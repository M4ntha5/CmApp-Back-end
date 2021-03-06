﻿using CmApp.Contracts;
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
            if (user.Password != user.Password2)
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
                Salt = Convert.ToBase64String(salt),
                Currency = user.Currency
            };

            var repo = new CodeMashRepository<UserEntity>(Client);

            var response = await repo.InsertOneAsync(entity, new DatabaseInsertOneOptions());
            return response;
        }

        public async Task<UserEntity> GetUserById(string carId)
        {
            var repo = new CodeMashRepository<UserEntity>(Client);
            var filter = Builders<UserEntity>.Filter.Eq("_id", BsonObjectId.Create(carId));
            var user = await repo.FindOneAsync(filter, new DatabaseFindOneOptions());
            return user;
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
            var repo = new CodeMashRepository<UserEntity>(Client);

            var projection = Builders<UserEntity>.Projection
                .Include(x => x.Blocked)
                .Include(x => x.Currency)
                .Include(x => x.Email)
                .Include(x => x.Id)
                .Include(x => x.Role);

            var users = await repo.FindAsync<UserEntity>(x => !x.Deleted, projection, null,
                new DatabaseFindOptions());

            return users.Items;
        }

        public async Task BlockUser(string userId)
        {
            var repo = new CodeMashRepository<UserEntity>(Client);
            var updateDefinition = Builders<UserEntity>.Update.Set(x => x.Blocked, true);
            await repo.UpdateOneAsync(x => x.Id == userId, updateDefinition, new DatabaseUpdateOneOptions());
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
                Builders<UserEntity>.Update.Set("first_name", user.FirstName),
                Builders<UserEntity>.Update.Set("last_name", user.LastName),
                Builders<UserEntity>.Update.Set("sex", user.Sex),
                Builders<UserEntity>.Update.Set("country", user.Country),
                Builders<UserEntity>.Update.Set("born_date", user.BornDate),
            };

            var update = Builders<UserEntity>.Update.Combine(updates);

            await repo.UpdateOneAsync(x => x.Id == user.Id, update, new DatabaseUpdateOneOptions());
        }
        public async Task<UserEntity> GetUserByEmail(string email)
        {
            var repo = new CodeMashRepository<UserEntity>(Client);

            FilterDefinition<UserEntity>[] filters =
            {
                Builders<UserEntity>.Filter.Eq("deleted", false),
                Builders<UserEntity>.Filter.Eq("email", email),
            };
            var filer = Builders<UserEntity>.Filter.And(filters);

            var user = await repo.FindOneAsync(filer, new DatabaseFindOneOptions());

            return user;
        }
        public async Task ChangeEmailConfirmationFlag(string userId)
        {
            var repo = new CodeMashRepository<UserEntity>(Client);
            var update = Builders<UserEntity>.Update.Set("email_confirmed", true);
            await repo.UpdateOneAsync(userId, update, new DatabaseUpdateOneOptions());
        }

        public async Task ChangePassword(string userId, string password)
        {
            var repo = new CodeMashRepository<UserEntity>(Client);

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashedPass = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );

            UpdateDefinition<UserEntity>[] updates =
            {
                Builders<UserEntity>.Update.Set("password", hashedPass),
                Builders<UserEntity>.Update.Set("salt", Convert.ToBase64String(salt)),
            };

            var update = Builders<UserEntity>.Update.Combine(updates);

            await repo.UpdateOneAsync(userId, update, new DatabaseUpdateOneOptions());
        }

        public async Task ChangeUserRole(string userId, string role)
        {
            var repo = new CodeMashRepository<UserEntity>(Client);
            var update = Builders<UserEntity>.Update.Set("role", role);
            await repo.UpdateOneAsync(userId, update, new DatabaseUpdateOneOptions());
        }

        public async Task DeleteUser(string userId)
        {
            var repo = new CodeMashRepository<UserEntity>(Client);
            var update = Builders<UserEntity>.Update.Set("deleted", true);
            await repo.UpdateOneAsync(userId, update, new DatabaseUpdateOneOptions());
        }

        public async Task DeleteResetToken(string resetId)
        {
            var repo = new CodeMashRepository<PasswordResetEntity>(Client);
            await repo.DeleteOneAsync(resetId);
        }

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
