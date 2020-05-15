using CmApp;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace UsersTestsTests
{
    class UserServiceTests
    {
        AuthService authService;
        UserRepository userRepo;
        [SetUp]
        public void Setup()
        {
            userRepo = new UserRepository();
            authService = new AuthService()
            {
                UserRepository = userRepo,
                EmailRepository = new EmailRepository(),
            };


        }

        [Test]
        public async Task InsertUser()
        {
            var user = new User("user10@user.com", "password", "password", "EUR");

            var resut = await userRepo.InsertUser(user);
            Assert.AreEqual(user.Email, resut.Email);

            user = new User("user18@user.com", "password", "passw4ord", "EUR");
            Assert.ThrowsAsync<BusinessException>(async () =>
                await userRepo.InsertUser(user));

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await userRepo.InsertUser(null));
            Assert.ThrowsAsync<BusinessException>(async () =>
                await userRepo.InsertUser(new User("", "p", "p2", "")));
        }

        [Test]
        public async Task BlockUserAndUnblockUser()
        {
            var userId = "5ea974effb56e6922479d97a";
            await userRepo.BlockUser(userId);
            await userRepo.UnblockUser(userId);
        }

        [Test]
        public async Task GetAllUsers()
        {
            var users = await userRepo.GetAllUsers();
            Assert.AreNotEqual(0, users.Count);
        }

        [Test]
        public async Task UpdateUser()
        {
            var userId = "5ea974effb56e6922479d97a";
            var user = new UserEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Id = userId

            };
            await userRepo.UpdateUser(user);
        }

        [Test]
        public async Task GetUserById()
        {
            var user = await userRepo.GetUserById("5ea974effb56e6922479d97a");
            Assert.IsNotNull(user);
        }
        [Test]
        public async Task GetUserByEmail()
        {
            var user = await userRepo.GetUserByEmail("user10@user.com");
            Assert.IsNotNull(user);
        }

        [Test]
        public async Task ChangeUserRole()
        {
            var id = "5ea974effb56e6922479d97a";
            await userRepo.ChangeUserRole(id, "user");
        }
        [Test]
        public async Task ChangePassword()
        {
            var id = "5ea9770d925c568c0407584b";
            await userRepo.ChangePassword(id, "password");
        }
        [Test]
        public async Task ChangeEmailConfirmationFlag()
        {
            var id = "5ea9770d925c568c0407584b";
            await userRepo.ChangeEmailConfirmationFlag(id);
        }

        [Test]
        public async Task DeleteUser()
        {
            var id = "5ea974effb56e6922479d97a";
            await userRepo.DeleteUser(id);
        }

        [Test]
        public async Task InsertPasswordReset()
        {
            await userRepo.InsertPasswordReset(
                new PasswordResetEntity
                {
                    Token = "token",
                    User = "5ea93b953ebbca201071af71",
                    ValidUntil = new DateTime(2020, 05, 01),
                });
        }

        [Test]
        public async Task GetPasswordResetByToken()
        {
            var toke = await userRepo.GetPasswordResetByToken("token");
            Assert.NotNull(toke);
        }

        [Test]
        public void Tokens()
        {
            var user = new UserEntity
            {
                Email = "mantozerix@gmail.com",
                Role = "3",
                Currency = "EUR",
                Id = "5e92d6b981569e0004f1dbbf"
            };
            var token = authService.GenerateDefaultToken(user);
            Assert.NotNull(token);
            user.Role = "255";
            token = authService.GenerateAdminToken(user);
            Assert.NotNull(token);
        }

        [Test]
        public async Task CreatePasswordResetToken()
        {
            await authService.CreatePasswordResetToken("mantozerix@gmail.com");
        }

        [Test]
        public async Task ResetPassword()
        {
            var user = new User("mantozerix@gmail.com", "password", "password", "EUR");

            Assert.ThrowsAsync<BusinessException>(async () =>
                await authService.ResetPassword(user));

            user.Token = "bXTgmhy685hTGC7uTWyqGorJSLqLBZPuxkHFB/DdRZM=";
            user.Password2 = "as";
            Assert.ThrowsAsync<BusinessException>(async () =>
                await authService.ResetPassword(user));


            user.Password2 = "password";
            await authService.ResetPassword(user);

        }

        [Test]
        public async Task Login()
        {
            var user = new User("mantozerix@gmail.com", "password", "password", "EUR");
            await authService.Login(user);

            user = new User("mantas.daunoravicius@ktu.edu", "password", "password", "EUR");
            await authService.Login(user);

            user = new User("mantas.daunoravicius@ktu.edu", "passwor4d", "password", "EUR");
            Assert.ThrowsAsync<BusinessException>(async () =>
                await authService.Login(user));

            user = new User("mantas.daunoraicius@ktu.edu", "passwor4d", "password", "EUR");
            Assert.ThrowsAsync<BusinessException>(async () =>
                await authService.Login(user));

            user = new User("testUser@user.com", "passworod", "password", "EUR");
            Assert.ThrowsAsync<BusinessException>(async () =>
                await authService.Login(user));

            user = new User("egle.da@live.com", "passwor4d", "password", "EUR");
            Assert.ThrowsAsync<BusinessException>(async () =>
                await authService.Login(user));
        }

        [Test]
        public async Task Register()
        {
            var user = new User("mantas.daunoravicius@ktu.edu", "password", "password", "EUR");
            Assert.ThrowsAsync<BusinessException>(async () =>
                await authService.Register(user));

            user = new User("mantozerixasdaad@gmail.com", "password", "password", "EUR");
            await authService.Register(user);

        }
        [Test]
        public async Task ConfirmUserEmail()
        {
            Assert.ThrowsAsync<BusinessException>(async () =>
                await authService.ConfirmUserEmail("5e94b45c92597c056c2a0a97"));

            Assert.ThrowsAsync<BusinessException>(async () =>
                await authService.ConfirmUserEmail("5e9353f7650a91000420e8d1"));

            await authService.ConfirmUserEmail("5ea9770d925c568c0407584b");

        }
    }
}
