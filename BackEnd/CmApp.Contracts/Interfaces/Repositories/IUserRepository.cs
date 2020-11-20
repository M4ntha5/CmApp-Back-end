using CmApp.Contracts.Domains;
using CmApp.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<Entities.User> InsertUser(Domains.User user);
        Task<Entities.User> GetUserById(int carId);
        Task BlockUser(int userId);
        Task UnblockUser(int userId);
        Task UpdateUser(Entities.User user);
        Task<List<Entities.User>> GetAllUsers();
        Task<Entities.User> GetUserByEmail(string email);
        Task ChangeEmailConfirmationFlag(int userId);
        Task ChangePassword(int userId, string password);
        Task ChangeUserRole(int userId, string role);
        Task DeleteUser(int userId);
        Task DeleteResetToken(int resetId);
        Task InsertPasswordReset(PasswordReset resetEntity);
        Task<PasswordReset> GetPasswordResetByToken(string token);
    }

}
