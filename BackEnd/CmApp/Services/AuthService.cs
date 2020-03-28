using CmApp.Contracts;
using CmApp.Domains;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class AuthService : IAuthService
    {
        public IUserRepository UserRepository { get; set; }

        public async Task<string> Register(User user)
        { 
            var response = await UserRepository.GetUserByEmail(user.Email);
            if (response != null)
            {
                throw new BusinessException("User with this email already exists!");
            }

            await UserRepository.InsertUser(user);

            return null;
        }

        public async Task<JwtSecurityToken> Login(User userData)
        {
            var user = await UserRepository.GetUserByEmail(userData.Email);

            if (user.Id != null)
            {
                string hashedPass = Convert.ToBase64String(
                    KeyDerivation.Pbkdf2(
                        password: userData.Password,
                        salt: Convert.FromBase64String(user.Salt),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8
                    )
                );
                if (hashedPass == user.Password)
                {
                    if (user.Role == "user")
                    {
                        return GenerateDefaultToken(user.Id);
                    }
                    if (user.Role == "admin")
                    {
                        return GenerateAdminToken(user.Id);
                    }
                }
            }
            return null;
        }

        public JwtSecurityToken GenerateDefaultToken(string id)
        {
            Environment.SetEnvironmentVariable("TestUser", "this_is_user_key");

            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TestUser")));


            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // add claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "user"),
                new Claim(ClaimTypes.NameIdentifier, id)
            };

            //create token
            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "readers",
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signingCredentials,
                claims: claims

                );
            return token;
        }

        public JwtSecurityToken GenerateAdminToken(string id)
        {
            Environment.SetEnvironmentVariable("TestAdmin", "this_is_admin_key");

            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TestAdmin")));


            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // add claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.NameIdentifier, id)
            };

            //create token
            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "readers",
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signingCredentials,
                claims: claims
                );
            return token;
        }

    }
}
