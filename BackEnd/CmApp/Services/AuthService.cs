﻿using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class AuthService : IAuthService
    {
        public IUserRepository UserRepository { get; set; }
        public IEmailRepository EmailRepository { get; set; }
        public PasswordResetRepository PasswordResetRepository { get; set; }

        public async Task<UserEntity> Me(string userId)
        {
            var user = await UserRepository.GetUserById(userId);
            if (user == null)
                throw new BusinessException("No such user");
            return user;           
        }

        public async Task<bool> Register(User user)
        {
            user.Email = user.Email.ToLower();
            var response = await UserRepository.GetUserByEmail(user.Email);
            if (response != null)
                throw new BusinessException("User with this email already exists!");

            var insertedUser = await UserRepository.InsertUser(user);
            await EmailRepository.SendEmailConfirmationEmail(insertedUser.Email, insertedUser.Id);

            return insertedUser == null ? false : true;
        }

        public async Task<JwtSecurityToken> Login(User userData)
        {
            userData.Email = userData.Email.ToLower();
            var user = await UserRepository.GetUserByEmail(userData.Email);
            //check if email confirmed
            if (!user.EmailConfirmed)
                throw new BusinessException("You must confirm your email, before loging in!");
            if (user.Blocked)
                throw new BusinessException("Your accout has been blocked, please contact system administrato");

            if (user != null && user.Id != null)
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
                        return GenerateDefaultToken(user);
                    }
                    if (user.Role == "admin")
                    {
                        return GenerateAdminToken(user);
                    }
                }
                else
                    throw new BusinessException("Incorrect password!");
            }
            return null;
        }

        public JwtSecurityToken GenerateDefaultToken(UserEntity user)
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
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, user.Currency)
            };

            //create token
            var token = new JwtSecurityToken(
                issuer: "shrouded-ocean-70036.herokuapp.com",
                audience: "readers",
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signingCredentials,
                claims: claims

                );
            return token;
        }

        public JwtSecurityToken GenerateAdminToken(UserEntity user)
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
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, user.Currency)
            };

            //create token
            var token = new JwtSecurityToken(
                issuer: "shrouded-ocean-70036.herokuapp.com",
                audience: "readers",
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signingCredentials,
                claims: claims
                );
            return token;
        }

        public async Task ConfirmUserEmail(string token)
        {
            var user = await UserRepository.GetUserById(token);
            if (user.EmailConfirmed)
                throw new BusinessException("Email already confirmed!");
            if (user == null || user.Deleted)
                throw new BusinessException("No such a user!");

            await UserRepository.ChangeEmailConfirmationFlag(user.Id);
            await EmailRepository.SendWelcomeEmail(user.Email);
        }
        public async Task CreatePasswordResetToken(string email)
        {
            var user = await UserRepository.GetUserByEmail(email);
            if (user == null)
                throw new BusinessException("User with this email not registered");
            if (!user.EmailConfirmed)
                throw new BusinessException("User with this email not registered");

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string token = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: user.Id,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );

            //reset token will be valid for 2 hours
            var entity = new PasswordResetEntity
            {
                Token = token,
                ValidUntil = DateTime.UtcNow.AddHours(2),
                User = user.Id
            };

            await PasswordResetRepository.InsertPasswordReset(entity);
            await EmailRepository.SendPasswordResetEmail(email, token);
        }

        public async Task ResetPassword(User user)
        {
            if (user.Password != user.Password2)
                throw new BusinessException("Passwords do not match");

            var resetDetails = await PasswordResetRepository.GetPasswordResetByToken(user.Token);
            if (resetDetails == null)
                throw new BusinessException("Error handling your password change. Please try again");
            var currentDate = DateTime.UtcNow;

            if (resetDetails.ValidUntil < currentDate)
                throw new BusinessException("Your request has expired. Try one more time");
            if (resetDetails.Token != user.Token)
                throw new BusinessException("Error changing your password. Please try again");

            await UserRepository.ChangePassword(resetDetails.User, user.Password);
        }

    }
}
