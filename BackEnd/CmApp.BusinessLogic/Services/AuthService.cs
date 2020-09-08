using CmApp.Contracts;
using CmApp.Contracts.Domains;
using CmApp.Contracts.Entities;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Utils;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository UserRepository;
        private readonly IEmailRepository EmailRepository;

        public AuthService(IUserRepository userRepository, IEmailRepository emailRepository)
        {
            UserRepository = userRepository;
            EmailRepository = emailRepository;
        }

        public async Task<bool> Register(User user)
        {
            user.Email = user.Email.ToLower();
            var response = await UserRepository.GetUserByEmail(user.Email);
            if (response != null)
                throw new BusinessException("User with this email already exists!");

            var insertedUser = await UserRepository.InsertUser(user);
            await EmailRepository.SendEmailConfirmationEmail(insertedUser.Email, insertedUser.ID.ToString());

            return insertedUser == null ? false : true;
        }

        public async Task<JwtSecurityToken> Login(User userData)
        {
            userData.Email = userData.Email.ToLower();
            var user = await UserRepository.GetUserByEmail(userData.Email);
            if (user == null)
                throw new BusinessException("Such user does not exist");
            //check if email confirmed
            if (!user.EmailConfirmed)
                throw new BusinessException("You must confirm your email, before loging in!");
            if (user.Blocked)
                throw new BusinessException("Your accout has been blocked, please contact system administrator");

            if (user != null)
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
                        return GenerateDefaultToken(user);
                    if (user.Role == "admin")
                        return GenerateAdminToken(user);
                }
                else
                    throw new BusinessException("Incorrect password!");
            }
            return null;
        }

        public JwtSecurityToken GenerateDefaultToken(UserEntity user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Settings.UserKey));


            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // add claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, user.Currency)
            };

            //create token
            var token = new JwtSecurityToken(
                issuer: "shrouded-ocean-70036.herokuapp.com",
                audience: "readers",
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signingCredentials,
                claims: claims

                );
            return token;
        }

        public JwtSecurityToken GenerateAdminToken(UserEntity user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Settings.AdminKey));


            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // add claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, user.Currency)
            };

            //create token
            var token = new JwtSecurityToken(
                issuer: "shrouded-ocean-70036.herokuapp.com",
                audience: "readers",
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signingCredentials,
                claims: claims
                );
            return token;
        }

        public async Task ConfirmUserEmail(int token)
        {
            var user = await UserRepository.GetUserById(token);
            if (user == null || user.Deleted)
                throw new BusinessException("No such a user!");
            if (user.EmailConfirmed)
                throw new BusinessException("Email already confirmed!");

            await UserRepository.ChangeEmailConfirmationFlag(user.ID);
            await EmailRepository.SendWelcomeEmail(user.Email);
        }
        public async Task CreatePasswordResetToken(string email)
        {
            var user = await UserRepository.GetUserByEmail(email);
            if (user == null)
                throw new BusinessException("User with this email not registered");
            if (!user.EmailConfirmed)
                throw new BusinessException("User with this email not registered");

            Random random = new Random();

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var token = new string(Enumerable.Repeat(chars, 64)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            //reset token will be valid for 2 hours
            var entity = new PasswordResetEntity
            {
                Token = token,
                ValidUntil = DateTime.UtcNow.AddHours(2),
                User = user
            };

            await UserRepository.InsertPasswordReset(entity);
            await EmailRepository.SendPasswordResetEmail(email, token);
        }

        public async Task ResetPassword(User user)
        {
            if (user.Password != user.Password2)
                throw new BusinessException("Passwords do not match");

            byte[] bytes = Encoding.Default.GetBytes(user.Token);
            var token = Encoding.UTF8.GetString(bytes);

            var resetDetails = await UserRepository.GetPasswordResetByToken(token);

            if (resetDetails == null)
                throw new BusinessException("Error handling your password change. Please try again");
            var currentDate = DateTime.UtcNow;

            if (resetDetails.ValidUntil < currentDate)
                throw new BusinessException("Your request has expired. Try one more time");
            if (resetDetails.Token != user.Token)
                throw new BusinessException("Error changing your password. Please try again");

            await UserRepository.ChangePassword(resetDetails.User.ID, user.Password);
            await UserRepository.DeleteResetToken(resetDetails.ID);
        }

        public async Task ResetPassword(int userId, User user)
        {
            if (user.Password != user.Password2)
                throw new BusinessException("Passwords do not match");

            await UserRepository.ChangePassword(userId, user.Password);
        }

        public async Task<UserDetails> GetSelectedUser(int userId)
        {
            var user = await UserRepository.GetUserById(userId);
            var userDetails = new UserDetails
            {
                Email = user.Email,
                FirstName = user.FirstName,
                BornDate = user.BornDate,
                Country = user.Country,
                Currency = user.Currency,
                LastName = user.LastName,
                Sex = user.Sex
            };
            return userDetails;
        }

        public async Task UpdateUserDetails(int userId, UserDetails user)
        {
            var entity = new UserEntity
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BornDate = user.BornDate,
                Country = user.Country,
                Sex = user.Sex,
                ID = userId
            };
            await UserRepository.UpdateUser(entity);

        }

    }

}
