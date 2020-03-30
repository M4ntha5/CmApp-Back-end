using CmApp.Domains;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UsersTests.Integration
{
    class UserServiceTests
    {
        [SetUp]
        public void Setup()
        {
            Settings.ApiKey = Environment.GetEnvironmentVariable("ApiKey");
            Settings.CaptchaApiKey = Environment.GetEnvironmentVariable("CaptchaApiKey");
        }

        [Test]
        public async Task InsertUser()
        {
            var repo = new UserRepository();

            var cars = new List<CarEntity>
            {
                {new CarEntity { Id = "5e4c3136c0ae1700011b6d8e"} },
                {new CarEntity { Id = "5e4c2d3bc0ae17000119da0b"} }
            };

            /*var user = new User("jonas", "j@j.lt", "password", "password", false, cars);

            var resut = await repo.InsertUser(user);

            Assert.AreEqual(user.Name, resut.Name);
            Assert.AreEqual(user.Email, resut.Email);
            */
        }

        [Test]
        public async Task BlockUser()
        {
            var repo = new UserRepository();

            var userId = "5e59073125783800010101b9";
            await repo.BlockUser(userId);

        }
        [Test]
        public async Task UnblockUser()
        {
            var repo = new UserRepository();

            var userId = "5e59073125783800010101b9";
            await repo.UnblockUser(userId);
        }

        [Test]
        public async Task GetAllUsers()
        {
            var repo = new UserRepository();

            var users = await repo.GetAllUsers();
            Assert.AreNotEqual(0, users.Count);
        }

        [Test]
        public async Task GetAllBlockedUsers()
        {
            var repo = new UserRepository();

            var users = await repo.GetAllBlockedUsers();
            Assert.AreNotEqual(0, users.Count);
        }

        [Test]
        public async Task GetAllUnblockedUsers()
        {
            var repo = new UserRepository();

            var users = await repo.GetAllUnblockedUsers();

            Assert.AreNotEqual(0, users.Count);
        }
    }
}
