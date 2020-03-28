using CmApp.Domains;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IAuthService
    {
        Task<string> Register(User user);
        Task<JwtSecurityToken> Login(User userData);
        JwtSecurityToken GenerateDefaultToken(string id);
        JwtSecurityToken GenerateAdminToken(string id);

    }
}
