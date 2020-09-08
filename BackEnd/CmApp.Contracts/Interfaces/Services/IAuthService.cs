using CmApp.Contracts.Domains;
using CmApp.Contracts.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> Register(User user);
        Task<JwtSecurityToken> Login(User userData);
        JwtSecurityToken GenerateDefaultToken(UserEntity user);
        JwtSecurityToken GenerateAdminToken(UserEntity user);
        Task ConfirmUserEmail(int token);
        Task CreatePasswordResetToken(string email);
        Task ResetPassword(User user);
        Task ResetPassword(int userId, User user);
        Task<UserDetails> GetSelectedUser(int userId);
        Task UpdateUserDetails(int userId, UserDetails user);
    }
}
