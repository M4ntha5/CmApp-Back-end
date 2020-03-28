using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class UserService : IUserService
    {
        public IUserRepository UserRepository { get; set; }

        public async Task<UserEntity> InsertNewUser(User user)
        {
            if (user.Password != user.Password2)
                throw new BusinessException("Passwords do not match!!");

            var newUser = await UserRepository.InsertUser(user);

            return newUser;
        }

        public async Task<UserEntity> GetSelecteduser(string userId)
        {
            var user = await UserRepository.GetUserById(userId);
            return user;
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
            var users = await UserRepository.GetAllUsers();
            return users;
        }

        public async Task<List<UserEntity>> GetAllBlockedUsers()
        {
            var users = await UserRepository.GetAllBlockedUsers();
            return users;
        }

        public async Task<List<UserEntity>> GetAllUnblockedUsers()
        {
            var users = await UserRepository.GetAllUnblockedUsers();
            return users;
        }

        public async Task UpdateUserDetails(string userId, UserEntity user)
        {
            user.Id = userId;
            await UserRepository.UpdateUser(user);
        }


    }
}
