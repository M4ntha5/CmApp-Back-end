using CmApp.Domains;
using CmApp.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IAuthService
    {
        Task<bool> Register(User user);
        Task<JwtSecurityToken> Login(User userData);
        JwtSecurityToken GenerateDefaultToken(UserEntity user);
        JwtSecurityToken GenerateAdminToken(UserEntity user);
        Task ConfirmUserEmail(string token);
        Task CreatePasswordResetToken(string email);
        Task ResetPassword(User user);
        Task ResetPassword(string userId, User user);
    }
}
