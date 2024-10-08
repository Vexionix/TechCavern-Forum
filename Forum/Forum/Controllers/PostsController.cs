using Forum.API.Services;
using Forum.Core.Entities;
using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Services;
using Forum.Core.Models;
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
		public async Task<ActionResult<GetPostPageData>> GetPostById([FromRoute] int id, [FromQuery] int page = 1)
		{
			try
			{
                GetPostPageData post = await _postsService.GetPostById(id, page);
				Console.WriteLine(id);
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

        [HttpGet("{postId}/max-pages"), Authorize]
        public async Task<ActionResult<int>> GetMaxPagesForPost([FromRoute] int postId)
        {
            try
            {
                int maxPagesCount = await _postsService.GetMaxPagesForPost(postId, 10);
                return StatusCode(StatusCodes.Status200OK, maxPagesCount);

            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
            }
        }

        [HttpPatch("{postId}/views"), Authorize]
        public async Task<ActionResult> IncrementPostViews([FromRoute] int postId)
        {
            try
            {
                await _postsService.IncrementPostViews(postId);
                return StatusCode(StatusCodes.Status200OK);

            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
            }
        }

        [HttpGet("subcategory/{subcategoryId}"), Authorize] 
		public async Task<ActionResult<(List<PostListElementDto>, List<PostListElementDto>)>> GetPostsForSubcategory([FromRoute] int subcategoryId, [FromQuery] int page = 1)
		{
			try
			{
				(List<PostListElementDto> pinnedPosts, List<PostListElementDto> regularPosts) = await _postsService.GetPostsForSubcategory(subcategoryId, page, 10);
				return StatusCode(StatusCodes.Status200OK, new { pinnedPosts = pinnedPosts, regularPosts = regularPosts });
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

        [HttpGet("latest"), Authorize]
        public async Task<ActionResult<List<GetPostDisplayDataDto>>> GetLatestPosts()
        {
            try
            {
                List<GetPostDisplayDataDto> posts = await _postsService.GetLatestPosts();
                return StatusCode(StatusCodes.Status200OK, posts);
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
		public async Task<ActionResult> AddPost(PostCreateDto postCreateModel)
		{
			try
			{
				await _postsService.AddPost(postCreateModel);
				return StatusCode(StatusCodes.Status201Created);
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
		public async Task<ActionResult> EditPost([FromRoute] int postId, PostEditDto postEditModel)
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
		public async Task<ActionResult> DeletePost([FromRoute] int postId)
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
		public async Task<ActionResult> RemovePostByModeration([FromRoute] int postId)
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
		public async Task<ActionResult> UpdatePostPinStatus([FromRoute] int postId)
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
		public async Task<ActionResult> UpdatePostLockStatus([FromRoute] int postId)
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
