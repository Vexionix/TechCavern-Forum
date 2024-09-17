using Forum.Core.Interfaces.Services;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Forum.Core.Exceptions;

namespace Forum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IConfiguration _configuration;

		public AuthController(IAuthService authService, IConfiguration configuration)
		{
			_authService = authService;
			_configuration = configuration;
		}

		[HttpPost("register")]
		public async Task<ActionResult> RegisterUser([FromBody] UserRegister userRegisterBody)
		{
			try
			{
				await _authService.RegisterUser(userRegisterBody);
				return StatusCode(StatusCodes.Status201Created);
			}
			catch (BadRequestException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
			}
		}

		[HttpPost("login")]
		public async Task<ActionResult<string>> LoginUser([FromBody] UserLogin userLoginBody)
		{
			try
			{
				string token = await _authService.LoginUser(userLoginBody, _configuration.GetSection("AppSettings:Token").Value!, Response);
				return StatusCode(StatusCodes.Status200OK, token);
			}
			catch (BadRequestException)
			{
				return BadRequest("Invalid username or password");
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
			}
		}

		[HttpPost("refresh-token"), Authorize]
		public async Task<ActionResult<string>> RefreshToken()
		{
			try
			{
				var refreshToken = Request.Cookies["refreshToken"];
				var userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
				string token = await _authService.RefreshToken(refreshToken, userId, _configuration.GetSection("AppSettings:Token").Value!, Response);
				return Ok(token);
			}
			catch (BadRequestException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
			}
		}
	}
}
