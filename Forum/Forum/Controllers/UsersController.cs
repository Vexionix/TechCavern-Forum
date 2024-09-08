using Forum.Core.Entities;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserRepository _userRepository;
		private readonly IPasswordService _passwordService;

		public UsersController(IUserRepository userRepository, IPasswordService passwordService)
		{
			_userRepository = userRepository;
			_passwordService = passwordService;
		}

		[HttpGet]
		public async Task<ActionResult<List<User>>> GetUsers()
		{
			// for testing purposes but might be used in another form for an admin panel page with pagination
			// to allow editing certain details about users (inappropriate bios/pfps etc.) or give them special titles

			var users = await _userRepository.GetAllUsers();
			var userDtos = users.Select(user => new UserDto
			{
				Id = user.Id,
				Username = user.Username,
				Email = user.Email,
				Bio = user.Bio,
				SelectedTitle = user.SelectedTitle,
				Role = user.Role,
				CreatedAt = user.CreatedAt,
				Titles = user.Titles.Select(t => new TitleDto { Id = t.Id, Title = t.TitleName }).ToList()
			}).ToList();

			return Ok(userDtos);
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
		public async Task<ActionResult> LoginUser([FromBody] UserLogin userLoginBody)
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
			
			return StatusCode(StatusCodes.Status200OK);
		}
	}
}
