using CmApp.Domains;
using CmApp.Entities;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IUserService
    {
        Task<UserEntity> InsertNewUser(User user);
        Task<UserDetails> GetSelectedUser(string userId);
        Task UpdateUserDetails(string userId, UserDetails user);
    }
}
