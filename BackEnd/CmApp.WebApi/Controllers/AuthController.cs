using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using CmApp.Contracts.DTO;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailRepository _emailRepository;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService, IEmailRepository emailRepository, 
            IUserRepository userRepository)
        {
            _authService = authService;
            _emailRepository = emailRepository;
            _userRepository = userRepository;
        }


        // GET: api/Auth
        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            try
            {
                var jwt = _authService.Login(user);
                return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                await _authService.Register(user);

                return Ok($"Confirmation email has been sent to {user.Email} . " +
                     $"If you can't find it, check your spam folder");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("email/confirm/{token}")]
        public async Task<IActionResult> ConfirmEmail(int token)
        {
            try
            {
                await _authService.ConfirmUserEmail(token);
                return Ok("Your email confirmed successfully. Now you can log in");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("password/reset/token")]
        public async Task<IActionResult> CreatePasswordReset([FromBody] User user)
        {
            try
            {
                await _authService.CreatePasswordResetToken(user.Email);
                return Ok($"Email has been sent to {user.Email}, don't forget to check spam folder");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("password/reset")]
        public async Task<IActionResult> ResetPassword([FromBody] User data)
        {
            try
            {
                await _authService.ResetPassword(data);
                return Ok("Password has been successfully reset. You can now log in");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("email/resend/{email}")]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (!user.EmailConfirmed)
                await _emailRepository.SendEmailConfirmationEmail(user.Email, user.Id);
            else
                throw new BusinessException("Email already confirmed");

            return Ok("Confirmation email has been successfully sent");
        }
    }
}
