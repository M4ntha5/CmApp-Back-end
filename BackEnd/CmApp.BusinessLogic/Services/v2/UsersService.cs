using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CmApp.Contracts.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace CmApp.BusinessLogic.Services.v2
{
    public class UsersService : IUsersService
    {
        private readonly Context _context;

        public UsersService(Context context)
        {
            _context = context;
        }

        public Task BlockUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                throw new BusinessException("Cannot block user, because such user not found");
            
            user.Blocked = true;
            return _context.SaveChangesAsync();
        }

        public Task ChangeEmailConfirmationFlag(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                throw new BusinessException("Cannot confirm email, because user not found");

            user.EmailConfirmed = true;
            return _context.SaveChangesAsync();
        }

        public Task ChangePassword(int userId, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                throw new BusinessException("Cannot change password, because such user not found");

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashedPass = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );

            user.Password = hashedPass;
            user.Salt = Convert.ToBase64String(salt);
            return _context.SaveChangesAsync();
        }

        public async Task AddRoleToUser(int userId, int roleId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new BusinessException("Cannot add role, because user not found");

            await _context.UserRoles.AddAsync(new UserRole()
            {
                RoleId = roleId,
                UserId = userId
            });
            
            await _context.SaveChangesAsync();
        }


        public async Task ChangeUserRole(int userId, string role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if(user != null)
            {
                //user.Role = role;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteResetToken(int resetId)
        {
            var resetToken = await _context.PasswordResets.FirstOrDefaultAsync(x => x.Id == resetId);
            if (resetToken == null)
                throw new BusinessException("Cannot delete token, because token not found");
            
            _context.Remove(resetToken);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new BusinessException("Cannot delete user, because user not found");
            
            user.Deleted = true;
            await _context.SaveChangesAsync();
        }

        public Task<List<User>> GetAllUsers()
        {
            return _context.Users.Where(x => !x.Deleted).ToListAsync();
        }

        public Task<PasswordReset> GetPasswordResetByToken(string token)
        {
            return _context.PasswordResets.FirstOrDefaultAsync(x => x.Token == token);
        }

        public Task<User> GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.EmailConfirmed && x.Email == email);
        }

        public Task<User> GetUserById(int userId)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task InsertPasswordReset(PasswordReset resetEntity)
        {
            if(resetEntity != null)
            {
                await _context.AddAsync(resetEntity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task InsertUser(Contracts.DTO.User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Cannot insert user in db, because user is empty");
            if (user.Password != user.Password2)
                throw new BusinessException("Passwords do not match!!");

            //password hashing
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashedPass = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: user.Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );

            var entity = new User
            {
                Email = user.Email,
                Password = hashedPass,
                Salt = Convert.ToBase64String(salt),
                Currency = user.Currency
            };

           await _context.Users.AddAsync(entity);
        }

        public async Task UnblockUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                user.Blocked = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUser(int userId, Contracts.DTO.UserDetails userData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if(user != null)
            {
                user.Sex = userData.Sex;
                user.FirstName = userData.FirstName;
                user.LastName = userData.LastName;
                user.Country = userData.Country;
                user.BornDate = userData.BornDate;
                await _context.SaveChangesAsync();
            }
        }
    }
}