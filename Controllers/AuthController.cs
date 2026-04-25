using Microsoft.AspNetCore.Mvc;
using SuperMarketAPI.Interfaces;
using SuperMarketAPI.DTOs;


namespace SuperMarketAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegisterDTO dto)
    {
        try
        {
            var result = await _service.Register(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            
            return BadRequest(new {message = ex.Message});
        }
    }


    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var result = await _service.Login(dto);
        if (result == null)
            return Unauthorized(new { message = "Email o contraseña incorrectos" });

        return Ok(result);
    }
}