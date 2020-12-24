using CmApp.Contracts.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> Register(DTO.User user);
        Task<JwtSecurityToken> Login(DTO.User userData);
        JwtSecurityToken GenerateDefaultToken(Models.User user);
        JwtSecurityToken GenerateAdminToken(Models.User user);
        Task ConfirmUserEmail(int token);
        Task CreatePasswordResetToken(string email);
        Task ResetPassword(DTO.User user);
        Task ResetPassword(int userId, DTO.User user);
        Task<UserDetails> GetSelectedUser(int userId);
        Task UpdateUserDetails(int userId, UserDetails user);
    }
}
