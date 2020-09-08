using CmApp.Contracts.Domains;
using CmApp.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity> InsertUser(User user);
        Task<UserEntity> GetUserById(int carId);
        Task BlockUser(int userId);
        Task UnblockUser(int userId);
        Task UpdateUser(UserEntity user);
        Task<List<UserEntity>> GetAllUsers();
        Task<UserEntity> GetUserByEmail(string email);
        Task ChangeEmailConfirmationFlag(int userId);
        Task ChangePassword(int userId, string password);
        Task ChangeUserRole(int userId, string role);
        Task DeleteUser(int userId);
        Task DeleteResetToken(int resetId);
        Task InsertPasswordReset(PasswordResetEntity resetEntity);
        Task<PasswordResetEntity> GetPasswordResetByToken(string token);
    }

}
