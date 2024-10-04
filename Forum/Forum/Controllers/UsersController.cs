using Forum.API.Services;
using Forum.Core.Entities;
using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Core.Models;
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
		private readonly IUsersService _usersService;

		public UsersController(IUserRepository userRepository, IUsersService usersService)
		{
			_userRepository = userRepository;
			_usersService = usersService;
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

		[HttpGet("{userId}"), Authorize]
		public async Task<ActionResult<User>> GetUserById([FromRoute] int userId)
		{
			try
			{
				User user = await _usersService.GetUserById(userId);
				return StatusCode(StatusCodes.Status200OK, user);
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

		[HttpGet("{userId}/username"), Authorize]
		public async Task<ActionResult<string>> GetUsernameForUserById([FromRoute] int userId)
		{
			try
			{
				string username = await _usersService.GetUsernameForUser(userId);
				return StatusCode(StatusCodes.Status200OK, username);
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

		[HttpGet("active"), Authorize]
		public async Task<ActionResult<int>> GetActiveUsers()
		{
			try
			{
				int activeUsers = await _usersService.GetActiveUsersNumber();
				return StatusCode(StatusCodes.Status200OK, activeUsers);
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

        [HttpPatch("{userId}/activity"), Authorize]
        public async Task<ActionResult> UpdateActiveStatus([FromRoute] int userId, [FromBody] UserStatusDto userStatusModel)
        {
            try
            {
                await _usersService.UpdateActiveStatus(userId, userStatusModel.IsActive);
                return StatusCode(StatusCodes.Status200OK);
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
