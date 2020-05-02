﻿using CmApp.Domains;
using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IUserRepository
    {
        Task<UserEntity> InsertUser(User user);
        Task<UserEntity> GetUserById(string carId);
        Task BlockUser(string userId);
        Task UnblockUser(string userId);
        Task UpdateUser(UserEntity user);
        Task<List<UserEntity>> GetAllUsers();
        Task<List<UserEntity>> GetAllBlockedUsers();
        Task<List<UserEntity>> GetAllUnblockedUsers();
        Task<UserEntity> GetUserByEmail(string email);
        Task ChangeEmailConfirmationFlag(string userId);
        Task ChangePassword(string userId, string password);
        Task ChangeUserRole(string userId, string role);
        Task DeleteUser(string userId);
        Task DeleteResetToken(string resetId);
    }

}
