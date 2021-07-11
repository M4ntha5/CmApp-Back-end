using CmApp.Contracts.DTO;
using CmApp.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task InsertUser(Contracts.DTO.User user);
        Models.User GetUserById(int carId);
        Task BlockUser(int userId);
        Task UnblockUser(int userId);
        Task UpdateUser(int userId, Contracts.DTO.UserDetails userData);
        Task<List<Models.User>> GetAllUsers();
        Models.User GetUserByEmail(string email);
        Task ChangeEmailConfirmationFlag(int userId);
        Task ChangePassword(int userId, string password);
        Task ChangeUserRole(int userId, string role);
        Task DeleteUser(int userId);
        Task DeleteResetToken(int resetId);
        Task InsertPasswordReset(PasswordReset resetEntity);
        Task<PasswordReset> GetPasswordResetByToken(string token);

        IQueryable<string> GetUserRoles(int userId);
        bool CheckIfPasswordResetTokenExists(string token);
    }

}
