using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
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
                FirstName = user.FirstName,
                BornDate = user.BornDate,
                Country = user.Country,
                Currency = user.Currency,
                LastName = user.LastName,
                Sex = user.Sex
            };
            return userDetails;
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
