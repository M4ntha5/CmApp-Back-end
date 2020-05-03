using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{
    [Route("api/users")]
    [Authorize(Roles = "user, admin", AuthenticationSchemes = "user, admin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static readonly IUserRepository userRepository = new UserRepository();
        private readonly IUserService UserService = new UserService()
        {
            UserRepository = userRepository
        };
        private readonly IAuthService authservice = new AuthService()
        {
            UserRepository = userRepository,
            PasswordResetRepository = new PasswordResetRepository(),
            EmailRepository = new EmailRepository()
        };

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (role != "admin")
                    throw new Exception("You can not access this resource!");

                var users = await userRepository.GetAllUsers();
                
                return Ok(users);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Users/5
        [HttpGet("{userId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (authUserId != userId && role != "admin")
                    throw new Exception("You can not access this resource!");

                var user = await UserService.GetSelectedUser(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                var newUser = await UserService.InsertNewUser(user);

                var userDetails = new UserDetails
                {
                    Email = newUser.Email,
                    FirstName = newUser.FirstName,
                    Currency = newUser.Currency
                };

                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT: api/Users/5
        [HttpPut("{userId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Put(string userId, [FromBody] UserDetails user)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (authUserId != userId && role != "admin")
                    throw new Exception("You can not access this resource!");

                await UserService.UpdateUserDetails(userId, user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Route("block/{userid}")]
        [HttpGet("block/{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> BlockUser(string userId)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (authUserId != userId && role != "admin")
                    throw new Exception("You can not access this resource!");

                await userRepository.BlockUser(userId);
                return Ok("Selected user scuccessfully blocked!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpGet("unblock/{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UnblockUser(string userId)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (authUserId != userId && role != "admin")
                    throw new Exception("You can not access this resource!");

                await userRepository.UnblockUser(userId);
                return Ok("Selected user scuccessfully unblocked!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[Route("delete/{userid}")]
        [HttpGet("delete/{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (authUserId != userId && role != "admin")
                    throw new Exception("You can not access this resource!");

                await userRepository.DeleteUser(userId);
                return Ok("Selected user scuccessfully deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{userId}/role")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeRole(string userId, [FromBody] User data)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (authUserId != userId && userRole != "admin")
                    throw new Exception("You can not access this resource!");

                await userRepository.ChangeUserRole(userId, data.Role);
                return Ok("Role sccessfully changed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("/api/users/{userId}/password/reset")]
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> ChangePassword(string userId, [FromBody] User data)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (authUserId != userId)
                    throw new Exception("You can not access this resource!");

                await authservice.ResetPassword(userId, data);
                return Ok("Password sccessfully changed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
