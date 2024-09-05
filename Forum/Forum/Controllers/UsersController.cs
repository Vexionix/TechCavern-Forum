﻿using Forum.Core.Entities;
using Forum.Data.Repositories;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IForumRepository _forumRepository;

		public UsersController(IForumRepository forumRepository)
		{
			_forumRepository = forumRepository;
		}

		[HttpGet]
		public ActionResult<List<User>> GetUsers()
		{
			// for testing purposes but might be used in another form for an admin panel page with pagination
			// to allow editing certain details about users (inappropriate bios/pfps etc.) or give them special titles

			var users = _forumRepository.GetAllUsers().Select(x => new UserDto()
			{
				Id = x.Id,
				Username = x.Username,
				Email = x.Email,
				Bio = x.Bio,
				SelectedTitle = x.SelectedTitle,
				Role = x.Role,
				CreatedAt = x.CreatedAt,
				Titles = _forumRepository.GetTitlesForUser(x.Id).Select(t=> new TitleDto() { Id = t.Id, Title = t.TitleName }).ToList()
			});

			return Ok(users);
		}

		[HttpPost("register")]
		public ActionResult RegisterUser([FromBody] UserRegister userRegisterBody)
		{
			if (_forumRepository.DoesUserWithUsernameOrEmailExist(userRegisterBody.Username, userRegisterBody.Email) == true)
			{
				return StatusCode(StatusCodes.Status412PreconditionFailed, "User with the same username or email already exists");
			}

			var newUser = new User(userRegisterBody.Username, 
				userRegisterBody.Email, 
				userRegisterBody.Password, // add encryption
				"Member", 
				"We don't know much about them, but we are sure they are cool.", 
				"Romania");

			_forumRepository.AddUser(newUser);

			return StatusCode(StatusCodes.Status201Created);
		}

		[HttpPost("login")]
		public ActionResult LoginUser([FromBody] UserLogin userLoginBody)
		{
			//var user = new User();
			//TODO properly (take entity from db to compare to the given input and return a response based on that)
			return StatusCode(StatusCodes.Status200OK);
		}
	}
}
