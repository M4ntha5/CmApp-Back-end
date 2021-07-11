using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CmApp.Contracts.DTO;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Models;
using CmApp.Utils;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CmApp.BusinessLogic.Services.v2
{
    public class AuthService : IAuthService
    { 
        private readonly IUserRepository _userRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly Context _context;

        public AuthService(Context context, IUserRepository userRepository, IEmailRepository emailRepository)
        {
            _userRepository = userRepository;
            _emailRepository = emailRepository;
            _context = context;
        }

        public async Task<bool> Register(Contracts.DTO.User user)
        {
            user.Email = user.Email.ToLower();
            var userExists = await _context.Users.AnyAsync(x => x.Email == user.Email);
            if (userExists)
                throw new BusinessException("User with this email already exists!");

            await _userRepository.InsertUser(user);
            //await EmailRepository.SendEmailConfirmationEmail(insertedUser.Email, insertedUser.ID.ToString());

            return true; //insertedUser == null ? false : true;
        }

        public async Task<JwtSecurityToken> Login(Contracts.DTO.User userData)
        {
            userData.Email = userData.Email.ToLower();
            var user = await _context.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == userData.Email);
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
            {
                var currRoles = user.Roles.Select(x => x.Role?.Name).ToList();
                if (currRoles.Contains("user"))
                    return GenerateDefaultToken(user);
                if (currRoles.Contains("admin"))
                    return GenerateAdminToken(user);
            }
            else
                throw new BusinessException("Incorrect password!");
            
            return null;
        }

        public JwtSecurityToken GenerateDefaultToken(Contracts.Models.User user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Settings.UserKey));


            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // add claims
            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Role, user.Roles),
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

        public JwtSecurityToken GenerateAdminToken(Contracts.Models.User user)
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
            return token;
        }

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

            Random random = new Random();

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var token = new string(Enumerable.Repeat(chars, 64)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            //reset token will be valid for 2 hours
            var entity = new PasswordReset
            {
                Token = token,
                ValidUntil = DateTime.UtcNow.AddHours(2),
                User = user
            };

            await _userRepository.InsertPasswordReset(entity);
            //await EmailRepository.SendPasswordResetEmail(email, token);
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

        public async Task ResetPassword(int userId, Contracts.DTO.User user)
        {
            if (user.Password != user.Password2)
                throw new BusinessException("Passwords do not match");

            await _userRepository.ChangePassword(userId, user.Password);
        }

        public async Task<UserDetails> GetSelectedUser(int userId)
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

        public async Task UpdateUserDetails(int userId, UserDetails user)
        {
            await _userRepository.UpdateUser(userId, user);
        }
    }
}