using Forum.Core.Entities;
using Forum.Core.Interfaces.Repositories;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserRepository _userRepository;

		public UsersController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
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
				return StatusCode(StatusCodes.Status412PreconditionFailed, "User with the same username or email already exists");
			}

			var newUser = new User(userRegisterBody.Username,
				userRegisterBody.Email,
				userRegisterBody.Password, // add encryption
				"Member",
				"We don't know much about them, but we are sure they are cool.",
				"Romania");

			await _userRepository.AddUser(newUser);

			return StatusCode(StatusCodes.Status201Created);
		}

		[HttpPost("login")]
		public async Task<ActionResult> LoginUser([FromBody] UserLogin userLoginBody)
		{
			//var user = new User();
			//TODO properly (take entity from db to compare to the given input and return a response based on that)
			return StatusCode(StatusCodes.Status200OK);
		}
	}
}
