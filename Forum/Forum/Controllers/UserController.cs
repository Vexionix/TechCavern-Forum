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
				Description = "We don't know much about them, but we are sure they are cool.",
				Title = "Member",
				Role = "Member",
				CreatedAt = DateTime.Now
			};
			return StatusCode(StatusCodes.Status201Created, newUser);
		}
	}
}
