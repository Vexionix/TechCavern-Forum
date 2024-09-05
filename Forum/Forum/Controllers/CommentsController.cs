using Forum.Core.Entities;
using Forum.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	[Route("api/[controller]/{postId}")]
	[ApiController]
	public class CommentsController : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<Comment>> GetCommentsForPost([FromRoute] int postId)
		{
			var comments = new List<Comment>();
			//TODO
			return comments;
		}
	}
}
