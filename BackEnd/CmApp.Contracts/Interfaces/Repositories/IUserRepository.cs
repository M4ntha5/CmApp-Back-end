using CmApp.Contracts.Domains;
using CmApp.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task InsertUser(Contracts.Domains.User user);
        Task<Models.User> GetUserById(int carId);
        Task BlockUser(int userId);
        Task UnblockUser(int userId);
        Task UpdateUser(int userId, Contracts.Domains.UserDetails userData);
        Task<List<Models.User>> GetAllUsers();
        Task<Models.User> GetUserByEmail(string email);
        Task ChangeEmailConfirmationFlag(int userId);
        Task ChangePassword(int userId, string password);
        Task ChangeUserRole(int userId, string role);
        Task DeleteUser(int userId);
        Task DeleteResetToken(int resetId);
        Task InsertPasswordReset(PasswordReset resetEntity);
        Task<PasswordReset> GetPasswordResetByToken(string token);
    }

}
