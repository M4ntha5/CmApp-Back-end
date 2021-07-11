using CmApp.Contracts;
using CmApp.Contracts.DTO;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
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
        private readonly IUserRepository _userRepository;
        private readonly IEmailRepository _emailRepository;

        public AuthService(IUserRepository userRepository, IEmailRepository emailRepository)
        {
            _userRepository = userRepository;
            _emailRepository = emailRepository;
        }

        public Task Register(Contracts.DTO.User user)
        {
            user.Email = user.Email.ToLower();
            var response = _userRepository.GetUserByEmail(user.Email);
            if (response != null)
                throw new BusinessException("User with this email already exists!");

            return _userRepository.InsertUser(user);
            //await EmailRepository.SendEmailConfirmationEmail(insertedUser.Email, insertedUser.ID.ToString());

            //return true; //insertedUser == null ? false : true;
        }

        public JwtSecurityToken Login(Contracts.DTO.User userData)
        {
            userData.Email = userData.Email.ToLower();
            var user = _userRepository.GetUserByEmail(userData.Email);
            if (user == null)
                throw new BusinessException("Such user does not exist");
            //check if email confirmed
            if (!user.EmailConfirmed)
                throw new BusinessException("You must confirm your email, before login in!");
            if (user.Blocked)
                throw new BusinessException("Your account has been blocked, please contact system administrator");

     
            var hashedPass = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: userData.Password,
                    salt: Convert.FromBase64String(user.Salt),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );
            if (hashedPass == user.Password)
                return GenerateToken(user);
    
            throw new BusinessException("Incorrect password!");
        }

        public JwtSecurityToken GenerateToken(Contracts.Models.User user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Settings.UserKey));


            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var userRoles = _userRepository.GetUserRoles(user.Id);
            var joinedRoles = string.Join(',', userRoles);
            // add claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, joinedRoles),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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

        /*public Task<JwtSecurityToken> GenerateAdminToken(Contracts.Models.User user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Settings.AdminKey));
            
            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // add claims
            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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
            return Task.FromResult(token);
        }*/

        public async Task ConfirmUserEmail(int token)
        {
            var user = _userRepository.GetUserById(token);
            if (user == null || user.Deleted)
                throw new BusinessException("No such a user!");
            if (user.EmailConfirmed)
                throw new BusinessException("Email already confirmed!");

            await _userRepository.ChangeEmailConfirmationFlag(user.Id);
            await _emailRepository.SendWelcomeEmail(user.Email);
        }
        public async Task CreatePasswordResetToken(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
                throw new BusinessException("User with this email not registered");
            if (!user.EmailConfirmed)
                throw new BusinessException("User with this email not registered");

            var random = new Random();

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            //recreate token until unique
            var exists = true;
            string token = string.Empty;
            while (exists)
            {
                token = new string(Enumerable.Repeat(chars, 64)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                exists = _userRepository.CheckIfPasswordResetTokenExists(token);
            }
            
            //reset token will be valid for 2 hours
            var entity = new PasswordReset
            {
                Token = token,
                ValidUntil = DateTime.UtcNow.AddHours(2),
                User = user
            };

            await _userRepository.InsertPasswordReset(entity);
            //await _emailRepository.SendPasswordResetEmail(email, token);
        }

        public async Task ResetPassword(Contracts.DTO.User user)
        {
            if (user.Password != user.Password2)
                throw new BusinessException("Passwords do not match");

            byte[] bytes = Encoding.Default.GetBytes(user.Token);
            var token = Encoding.UTF8.GetString(bytes);

            var resetDetails = await _userRepository.GetPasswordResetByToken(token);

            if (resetDetails == null)
                throw new BusinessException("Error handling your password change. Please try again");
            var currentDate = DateTime.UtcNow;

            if (resetDetails.ValidUntil < currentDate)
                throw new BusinessException("Your request has expired. Try one more time");
            if (resetDetails.Token != user.Token)
                throw new BusinessException("Error changing your password. Please try again");

            await _userRepository.ChangePassword(resetDetails.User.Id, user.Password);
            await _userRepository.DeleteResetToken(resetDetails.Id);
        }

        public Task ResetPassword(int userId, Contracts.DTO.User user)
        {
            if (user.Password != user.Password2)
                throw new BusinessException("Passwords do not match");

            return _userRepository.ChangePassword(userId, user.Password);
        }

        public UserDetails GetSelectedUser(int userId)
        {
            var user = _userRepository.GetUserById(userId);
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

        public Task UpdateUserDetails(int userId, UserDetails user)
        {
            return _userRepository.UpdateUser(userId, user);
        }

    }

}
