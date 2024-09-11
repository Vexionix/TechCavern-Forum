using Forum.Core.Entities;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
				"Member",
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
			
			return StatusCode(StatusCodes.Status200OK, CreateToken(user));
		}

		private string CreateToken(User user)
		{
			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, user.Username)
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
