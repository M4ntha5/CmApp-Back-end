using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.X509;

namespace CmApp.BusinessLogic.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public async Task BlockUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if(user != null)
            {
                user.Blocked = true;
                await _context.SaveChangesAsync();
            }
        }

        public Task ChangeEmailConfirmationFlag(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                throw new BusinessException("Cannot confirm email, because user not found");
           
            user.EmailConfirmed = true;
            return _context.SaveChangesAsync();
        }

        public async Task ChangePassword(int userId, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new BusinessException("Such user does not exist");

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
            await _context.SaveChangesAsync();
        }

        public async Task ChangeUserRole(int userId, string role)
        {
            throw new NotImplementedException();
            
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if(user != null)
            {
               // user.Role = role;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteResetToken(int resetId)
        {
            var resetToken = await _context.PasswordResets.FirstOrDefaultAsync(x => x.Id == resetId);
            if(resetToken != null)
            {
                _context.Remove(resetToken);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if(user != null)
            {
                user.Deleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public Task<List<User>> GetAllUsers()
        {
            return _context.Users.Where(x => !x.Deleted).ToListAsync();
        }

        public Task<PasswordReset> GetPasswordResetByToken(string token)
        {
            return _context.PasswordResets.FirstOrDefaultAsync(x => x.Token == token);
        }
        public IQueryable<string> GetUserRoles(int userId)
        {
            return _context.UserRoles
                .Include(x => x.Role)
                .Where(x => x.UserId == userId && x.Role != null)
                .Select(x => x.Role.Name)
                .AsQueryable();
        }

        public bool CheckIfPasswordResetTokenExists(string token)
        {
            return _context.PasswordResets.Any(x => x.Token == token);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users
                .Include(x => x.Roles)
                .FirstOrDefault(x => x.EmailConfirmed && x.Email == email);
        }

        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(x => x.Id == userId);
        }

        public async Task InsertPasswordReset(PasswordReset resetEntity)
        {
            if(resetEntity != null)
            {
                await _context.AddAsync(resetEntity);
                await _context.SaveChangesAsync();
            }
        }

        public Task InsertUser(Contracts.DTO.User user)
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

            var hashedPass = Convert.ToBase64String(
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

           _context.Users.Add(entity);
           return _context.SaveChangesAsync();
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
