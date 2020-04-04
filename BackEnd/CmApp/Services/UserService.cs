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

        public async Task<UserDetails> GetSelectedUser(string userId)
        {
            var user = await UserRepository.GetUserById(userId);
            var userDetails = new UserDetails
            {
                Email = user.Email,
                FirstName = user.FirstName
            };
            return userDetails;
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

        public async Task UpdateUserDetails(string userId, UserDetails user)
        {
            var entity = new UserEntity
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BornDate = user.BornDate,
                Country = user.Country,
                Sex = user.Sex,
                Id = userId
            };
            await UserRepository.UpdateUser(entity);
        }


    }
}
