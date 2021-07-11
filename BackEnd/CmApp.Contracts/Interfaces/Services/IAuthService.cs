using CmApp.Contracts.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface IAuthService
    {
        Task Register(DTO.User user);
        JwtSecurityToken Login(DTO.User userData);
        JwtSecurityToken GenerateToken(Models.User user);
        Task ConfirmUserEmail(int token);
        Task CreatePasswordResetToken(string email);
        Task ResetPassword(DTO.User user);
        Task ResetPassword(int userId, DTO.User user);
        UserDetails GetSelectedUser(int userId);
        Task UpdateUserDetails(int userId, UserDetails user);
    }
}
