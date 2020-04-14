using CmApp;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Repositories;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace UsersTests.Integration
{
    class UserServiceTests
    {
        UserRepository userRepo;
        [SetUp]
        public void Setup()
        {
            userRepo = new UserRepository();
        }

        [Test]
        public async Task InsertUser()
        {
            var user = new User("testUser@user.com", "password", "password", "EUR");

            var resut = await userRepo.InsertUser(user);

            Assert.AreEqual(user.Currency, resut.Currency);
            Assert.AreEqual(user.Email, resut.Email);
        }

        [Test]
        public async Task BlockUser()
        {
            var userId = "5e94b45c9b897c056c2a0a97";
            await userRepo.BlockUser(userId);
        }
        [Test]
        public async Task UnblockUser()
        { 
            var userId = "5e94b45c9b897c056c2a0a97";
            await userRepo.UnblockUser(userId);
        }

        [Test]
        public async Task GetAllUsers()
        {
            var users = await userRepo.GetAllUsers();
            Assert.AreNotEqual(0, users.Count);
        }

        [Test]
        public async Task GetAllBlockedUsers()
        {
            var users = await userRepo.GetAllBlockedUsers();
            Assert.AreNotEqual(0, users.Count);
        }

        [Test]
        public async Task GetAllUnblockedUsers()
        {
            var users = await userRepo.GetAllUnblockedUsers();
            Assert.AreNotEqual(0, users.Count);
        }
        [Test]
        public void InsertEmpty()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async ()=> 
                await userRepo.InsertUser(null));
            Assert.ThrowsAsync<BusinessException>(async () =>
                await userRepo.InsertUser(new User("","p","p2","")));
        }

        [Test]
        public async Task UpdateUser()
        {
            var user = new UserEntity
            {
                Id = "5e94b45c9b897c056c2a0a97",
                FirstName = "kopnas"
            };
            await userRepo.UpdateUser(user);
        }
        [Test]
        public async Task GetUserById()
        {
            var user = await userRepo.GetUserById("5e94b45c9b897c056c2a0a97");
            Assert.IsNotNull(user);
        }
        [Test]
        public async Task GetUserByEmail()
        {
            var user = await userRepo.GetUserByEmail("testUser@user.com");
            Assert.IsNotNull(user);
        }

        [Test]
        public async Task ChangeUserRole()
        {
            var id = "5e94b45c9b897c056c2a0a97";
            await userRepo.ChangeUserRole(id, "user");
        }
        [Test]
        public async Task ChangePassword()
        {
            var id = "5e94b45c9b897c056c2a0a97";
            await userRepo.ChangePassword(id, "password");
        }
        [Test]
        public async Task ChangeEmailConfirmationFlag()
        {
            var id = "5e94b45c9b897c056c2a0a97";
            await userRepo.ChangeEmailConfirmationFlag(id);
        }

        [Test]
        public async Task DeleteUser()
        {
            var id = "5e94c89f431ffe0001806e40";
            await userRepo.DeleteUser(id);
        }
    }
}
