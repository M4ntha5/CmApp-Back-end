using CmApp.Domains;
using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IUserService
    {
        Task<UserEntity> InsertNewUser(User user);
        Task<UserEntity> GetSelecteduser(string userId);
        Task<List<UserEntity>> GetAllUsers();
        Task<List<UserEntity>> GetAllBlockedUsers();
        Task<List<UserEntity>> GetAllUnblockedUsers();
        Task UpdateUserDetails(string userId, UserEntity user);
    }
}
