using CmApp.Contracts.Domains;
using CmApp.Contracts.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> Register(Domains.User user);
        Task<JwtSecurityToken> Login(Domains.User userData);
        JwtSecurityToken GenerateDefaultToken(Entities.User user);
        JwtSecurityToken GenerateAdminToken(Entities.User user);
        Task ConfirmUserEmail(int token);
        Task CreatePasswordResetToken(string email);
        Task ResetPassword(Domains.User user);
        Task ResetPassword(int userId, Domains.User user);
        Task<UserDetails> GetSelectedUser(int userId);
        Task UpdateUserDetails(int userId, UserDetails user);
    }
}
