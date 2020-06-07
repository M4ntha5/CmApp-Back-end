using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace CmApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService = new AuthService()
        {
            UserRepository = new UserRepository(),
            EmailRepository = new EmailRepository()
        };

        // GET: api/Auth
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            try
            {
                var jwt = await authService.Login(user);
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
                var response = await authService.Register(user);

                return Ok($"Confirmation email has been sent to {user.Email} . " +
                     $"If you can't find it, check your spam folder");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("email/confirm/{token}")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            try
            {
                await authService.ConfirmUserEmail(token);
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
                await authService.CreatePasswordResetToken(user.Email);
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
                await authService.ResetPassword(data);
                return Ok("Password successfully reseted. You can now log in");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("email/resend/{email}")]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            var repo = new EmailRepository();
            var userRepo = new UserRepository();
            var user = await userRepo.GetUserByEmail(email);
            if(!user.EmailConfirmed)
                await repo.SendEmailConfirmationEmail(user.Email, user.Id);
            else
                throw new BusinessException("Email already confirmed");

            return Ok("Email has been successfully sent");
        }
    }
}
