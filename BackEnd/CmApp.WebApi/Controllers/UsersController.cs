using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CmApp.Contracts.DTO;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.WebApi.Controllers
{
    [Route("api/users")]
    [Authorize(Roles = "user, admin", AuthenticationSchemes = "user, admin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UsersController(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
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

                var users = await _userRepository.GetAllUsers();

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
        public IActionResult Get(int userId)
        {
            try
            {
                var authUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (int.Parse(authUserId) != userId && role != "admin")
                    throw new Exception("You can not access this resource!");

                var user = _authService.GetSelectedUser(userId);
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

                await _authService.UpdateUserDetails(userId, user);
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

                await _userRepository.BlockUser(userId);
                return Ok("Selected user successfully blocked!");
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

                await _userRepository.UnblockUser(userId);
                return Ok("Selected user successfully unblocked!");
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

                await _userRepository.DeleteUser(userId);
                return Ok("Selected user successfully deleted!");
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

                await _userRepository.ChangeUserRole(userId, data.Role);
                return Ok("Role successfully changed!");
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

                await _authService.ResetPassword(userId, data);
                return Ok("Password successfully changed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
