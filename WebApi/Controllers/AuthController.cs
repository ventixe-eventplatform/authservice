using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(request);

        if (!result.Success)
            return Unauthorized(new { Error = result.Error! });

        return Ok(result.Data);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequestModel request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Error = "Email and password do not match." });

        var result = await _authService.SignInAsync(request);

        if (!result.Success)
            return Unauthorized(new { Error = result.Error! });

        return Ok(result.Data);
    }

    [HttpPost("signout")]
    public async Task<IActionResult> SignOutAsync()
    {
        await _authService.SignOutAsync();
        return Ok();
    }

    [HttpPost("exists")]
    public async Task<IActionResult> EmailExists([FromBody] EmailRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.UserExistsAsync(request);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
