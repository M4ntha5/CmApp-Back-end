using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using CmApp.Domains;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService = new AuthService()
        {
            UserRepository = new UserRepository()
        };

        // GET: api/Auth
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            try
            {
                var jwt = await authService.Login(user);
                if (jwt == null)
                    return BadRequest("This user doesn't exist");

                return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
            }
            catch(Exception ex)
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
                if (response == null)
                    return Ok("Success");
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
