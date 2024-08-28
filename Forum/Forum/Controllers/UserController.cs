using Forum.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		[HttpPost]
		public ActionResult<User> CreateUser([FromBody] UserCreate userCreateBody)
		{
			var newUser = new User()
			{
				Id = 1,
				Username = userCreateBody.Username,
				Email = userCreateBody.Email,
				Password = userCreateBody.Password,
				Bio = "We don't know much about them, but we are sure they are cool.",
				Title = "Member",
				Role = "Member",
				CreatedAt = DateTime.Now
			};
			//TODO properly (add to db, check the fields and create a basic user etc)
			return StatusCode(StatusCodes.Status201Created, newUser);
		}
	}
}
