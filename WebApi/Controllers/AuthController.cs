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
    public async Task<IActionResult> Register(RegisterRequestModel request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(request);
        return result.Success ? Ok(result) : StatusCode(500, result.Error);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInRequestModel request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.SignInAsync(request);
        return result.Success ? Ok(result) : StatusCode(500, result.Error);
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
