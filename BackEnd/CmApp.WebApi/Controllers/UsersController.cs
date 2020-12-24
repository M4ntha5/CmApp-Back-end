using CmApp.Contracts;
using CmApp.Contracts.DTO;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CmApp.Controllers
{
    [Route("api/users")]
    [Authorize(Roles = "user, admin", AuthenticationSchemes = "user, admin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository UserRepository;
        private readonly IAuthService AuthService;

        public UsersController(IUserRepository userRepository, IAuthService authService)
        {
            UserRepository = userRepository;
            AuthService = authService;
        }


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

                var users = await UserRepository.GetAllUsers();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Users/5
        [HttpGet("{userId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Get(int userId)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (int.Parse(authUserId) != userId && role != "admin")
                    throw new Exception("You can not access this resource!");

                var user = await AuthService.GetSelectedUser(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT: api/Users/5
        [HttpPut("{userId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Put(int userId, [FromBody] UserDetails user)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (int.Parse(authUserId) != userId && role != "admin")
                    throw new Exception("You can not access this resource!");

                await AuthService.UpdateUserDetails(userId, user);
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
        public async Task<IActionResult> BlockUser(int userId)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (int.Parse(authUserId) != userId && role != "admin")
                    throw new Exception("You can not access this resource!");

                await UserRepository.BlockUser(userId);
                return Ok("Selected user scuccessfully blocked!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("unblock/{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UnblockUser(int userId)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (int.Parse(authUserId) != userId && role != "admin")
                    throw new Exception("You can not access this resource!");

                await UserRepository.UnblockUser(userId);
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
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (int.Parse(authUserId) != userId && role != "admin")
                    throw new Exception("You can not access this resource!");

                await UserRepository.DeleteUser(userId);
                return Ok("Selected user scuccessfully deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{userId}/role")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeRole(int userId, [FromBody] User data)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (int.Parse(authUserId) != userId && userRole != "admin")
                    throw new Exception("You can not access this resource!");

                await UserRepository.ChangeUserRole(userId, data.Role);
                return Ok("Role sccessfully changed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("/api/users/{userId}/password/reset")]
        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] User data)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (int.Parse(authUserId) != userId)
                    throw new Exception("You can not access this resource!");

                await AuthService.ResetPassword(userId, data);
                return Ok("Password sccessfully changed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
