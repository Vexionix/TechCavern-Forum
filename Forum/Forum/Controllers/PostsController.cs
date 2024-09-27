using Forum.Core.Entities;
using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Services;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostsController : ControllerBase
	{
		private readonly IPostsService _postsService;

		public PostsController(IPostsService postsService)
		{
			_postsService = postsService;
		}

		[HttpGet("{id}"), Authorize]
		public async Task<ActionResult<Post>> GetPostById([FromRoute] int id)
		{
			try
			{
				Post post = await _postsService.GetPostById(id);
				return StatusCode(StatusCodes.Status200OK, post);
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

		[HttpGet("added-today"), Authorize]
		public async Task<ActionResult<int>> GetPostsAddedToday()
		{
			try
			{
				int numberOfPosts = await _postsService.GetPostsAddedToday();
				return StatusCode(StatusCodes.Status200OK, numberOfPosts);
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

		[HttpPost, Authorize]
		public async Task<ActionResult<int>> AddPost(PostCreateDto postCreateModel)
		{
			try
			{
				await _postsService.AddPost(postCreateModel);
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

		[HttpPut("edit/{postId}"), Authorize]
		public async Task<ActionResult<int>> EditPost([FromRoute] int postId, PostEditDto postEditModel)
		{
			try
			{
				await _postsService.EditPost(postId, postEditModel);
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

		[HttpDelete("{postId}"), Authorize]
		public async Task<ActionResult<int>> DeletePost([FromRoute] int postId)
		{
			try
			{
				await _postsService.DeletePost(postId);
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

		[HttpDelete("remove/{postId}"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<int>> RemovePostByModeration([FromRoute] int postId)
		{
			try
			{
				await _postsService.ModerationRemovePost(postId);
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

		[HttpPatch("update-pin/{postId}"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<int>> UpdatePostPinStatus([FromRoute] int postId)
		{
			try
			{
				await _postsService.UpdatePinStatus(postId);
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

		[HttpPatch("update-lock/{postId}"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<int>> UpdatePostLockStatus([FromRoute] int postId)
		{
			try
			{
				await _postsService.UpdateLockStatus(postId);
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
