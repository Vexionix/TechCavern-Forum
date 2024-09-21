using Forum.Core.Entities;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserRepository _userRepository;

		public UsersController(IUserRepository userRepository, IPasswordService passwordService)
		{
			_userRepository = userRepository;
		}

		[HttpGet, Authorize(Roles = "Admin")]
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
	}
}
