using Forum.API.Models;
using Forum.Core.Entities;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Forum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserRepository _userRepository;
		private readonly IPasswordService _passwordService;
		private readonly IConfiguration _configuration;

		public AuthController(IUserRepository userRepository, IPasswordService passwordService, IConfiguration configuration)
		{
			_userRepository = userRepository;
			_passwordService = passwordService;
			_configuration = configuration;
		}

		[HttpPost("register")]
		public async Task<ActionResult> RegisterUser([FromBody] UserRegister userRegisterBody)
		{
			if (await _userRepository.UserAlreadyExists(userRegisterBody.Username, userRegisterBody.Email) == true)
			{
				return StatusCode(StatusCodes.Status400BadRequest, "User with the same username or email already exists");
			}

			if (!_passwordService.CheckCriteria(userRegisterBody.Password))
			{
				return StatusCode(StatusCodes.Status400BadRequest, "Password does not meet criteria.");
			}

			var newUser = new User(userRegisterBody.Username,
				userRegisterBody.Email,
				_passwordService.Encrypt(userRegisterBody.Password),
				"",
				"We don't know much about them, but we are sure they are cool.",
				"Romania");

			await _userRepository.AddUser(newUser);

			return StatusCode(StatusCodes.Status201Created);
		}

		[HttpPost("login")]
		public async Task<ActionResult<string>> LoginUser([FromBody] UserLogin userLoginBody)
		{
			var user = await _userRepository.GetUserByUsername(userLoginBody.Username);

			if (user is null)
			{
				return StatusCode(StatusCodes.Status400BadRequest, "No user with such username exists.");
			}

			if (!_passwordService.CheckMatchingHash(userLoginBody.Password, user.PasswordHash))
			{
				return StatusCode(StatusCodes.Status400BadRequest, "Wrong password.");
			}

			if (user.IsBanned)
			{
				return StatusCode(StatusCodes.Status400BadRequest, "You are trying to login to a banned account.");
			}

			var refreshToken = GenerateRefreshToken();
			SetRefreshToken(refreshToken);
			
			await _userRepository.AddRefreshToken(new RefreshToken(user.Id, refreshToken.Token, refreshToken.ExpiresAt)
			{
				CreatedAt = refreshToken.CreatedAt
			});

			return StatusCode(StatusCodes.Status200OK, CreateToken(user));
		}

		[HttpPost("refresh-token"), Authorize]
		public async Task<ActionResult<string>> RefreshToken()
		{
			var refreshToken = Request.Cookies["refreshToken"];

			Console.WriteLine(refreshToken);

			var userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

			User? user = await _userRepository.GetUserById(userId);

			if(user is null)
			{
				return BadRequest("User doesn't exist.");
			}

			if((await _userRepository.GetRefreshTokensForUserId(userId)).Any(x => x.Token == refreshToken))
			{
				await _userRepository.RemoveRefreshToken(refreshToken!);

				string token = CreateToken(user);
				var newRefreshToken = GenerateRefreshToken();
				SetRefreshToken(newRefreshToken);

				await _userRepository.AddRefreshToken(new RefreshToken(user.Id, newRefreshToken.Token, newRefreshToken.ExpiresAt)
				{
					CreatedAt = newRefreshToken.CreatedAt
				});

				return Ok(token);
			}

			return BadRequest("Invalid refresh token");
		}

		private RefreshTokenDto GenerateRefreshToken()
		{
			var refreshToken = new RefreshTokenDto
			{
				Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
				ExpiresAt = DateTime.Now.AddDays(7)
			};

			return refreshToken;
		}

		private void SetRefreshToken(RefreshTokenDto refreshToken)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = refreshToken.ExpiresAt
			};

			Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
		}

		private string CreateToken(User user)
		{
			if (user is null)
			{
				return "";
			}

			string role;

			switch (user.Role)
			{
				case Core.Enums.Role.Member:
					role = "Member";
					break;
				case Core.Enums.Role.Admin:
					role = "Admin";
					break;
				default:
					role = "defaultValue";
					break;
			}

			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Role, role)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds
				);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}
	}
}
