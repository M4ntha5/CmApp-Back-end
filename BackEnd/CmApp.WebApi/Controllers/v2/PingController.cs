using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.WebApi.Controllers.v2;

[Route("api/ping")]
[ApiController]
[AllowAnonymous]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Ping()
    {
        try
        {
            return Ok("Pong");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}