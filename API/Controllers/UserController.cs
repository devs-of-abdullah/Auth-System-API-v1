using Business;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
         readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _userService.Register(dto.Email, dto.Password);

            if(!success) return BadRequest("Error creating User");

            return Ok("User registered succesfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
              
            var token = await _userService.Login(dto.Email, dto.Password);

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Email or password is incorrect");

            return Ok(new
            {
                token
            });
        }
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
            });
        }


    }
}
