using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.DTO.CustomerDto;
using Field_Ops.Application.DTO.UserDto;
using Field_Ops.Application.Helper;
using Field_Ops.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Field_Ops.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.RegisterUserAsync(dto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.LoginAsync(dto);

            return StatusCode(response.StatusCode, response);
        }
    }
}
