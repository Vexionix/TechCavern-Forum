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
				return StatusCode(StatusCodes.Status200OK, new { token });
			}
			catch (BadRequestException)
			{
				return BadRequest("Invalid username or password");
            }
            catch (BannedUserException)
            {
                return BadRequest("The account you attempt to log into is banned!");
            }
            catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
			}
		}

		[HttpGet("refresh")]
		public async Task<ActionResult<string>> RefreshToken()
		{
			try
			{
				var authHeader = Request.Headers["Authorization"].ToString();
				var refreshToken = Request.Cookies["refreshToken"];
				string token = await _authService.RefreshToken(authHeader, refreshToken, _configuration.GetSection("AppSettings:Token").Value!, Response);
				return Ok(new { token });
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

		[HttpGet("logout")]
		public async Task<ActionResult> Logout()
		{
			try
			{
				var refreshToken = Request.Cookies["refreshToken"];
				await _authService.Logout(refreshToken, Response);
				return Ok();
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
