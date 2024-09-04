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
		public ActionResult RegisterUser([FromBody] UserRegister userRegisterBody)
		{
			var newUser = new User()
			{
				Username = userRegisterBody.Username,
				Email = userRegisterBody.Email,
				Password = userRegisterBody.Password,
				Bio = "We don't know much about them, but we are sure they are cool.",
				Title = "Member",
				Role = "Member",
				CreatedAt = DateTime.Now
			};
			//TODO properly (add to db, check the fields and create a basic user, check if user is already registered etc)
			return StatusCode(StatusCodes.Status201Created, newUser);
		}

		[HttpPost]
		public ActionResult LoginUser([FromBody] UserLogin userLoginBody)
		{
			var user = new User();
			//TODO properly (take entity from db to compare to the given input and return a response based on that)
			return StatusCode(StatusCodes.Status200OK);
		}
	}
}
