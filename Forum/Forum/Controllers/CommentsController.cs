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
	public class CommentsController : ControllerBase
	{
		private readonly ICommentsService _commentsService;
		
		public CommentsController(ICommentsService commentsService)
		{
			_commentsService = commentsService;
		}

		[HttpGet("{id}"), Authorize]
		public async Task<ActionResult<CommentGetDto>> GetCommentById([FromRoute] int id)
		{
			try
			{
                CommentGetDto comment = await _commentsService.GetCommentById(id);
				return StatusCode(StatusCodes.Status200OK, comment);
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

        [HttpGet("latest/{userId}"), Authorize]
        public async Task<ActionResult<List<GetCommentProfile>>> GetLatestCommentsForUser([FromRoute] int userId)
        {
            try
            {
                List<GetCommentProfile> comments = (await _commentsService.GetLatestCommentsForUser(userId)).ToList();
                return StatusCode(StatusCodes.Status200OK, comments);
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

        [HttpGet("post/{postId}"), Authorize]
		public async Task<ActionResult<List<Comment>>> GetCommentsForPosts([FromRoute] int postId)
		{
			try
			{
				List<Comment> comments = await _commentsService.GetCommentsForPost(postId);
				return StatusCode(StatusCodes.Status200OK, comments);
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
		public async Task<ActionResult> AddComment(CommentCreateDto commentCreateModel)
		{
			try
			{
				await _commentsService.AddComment(commentCreateModel);
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

		[HttpPut("edit/{commentId}"), Authorize]
		public async Task<ActionResult> EditComment([FromRoute] int commentId, CommentEditDto commentEditModel)
		{
			try
			{
				await _commentsService.EditComment(commentId, commentEditModel);
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

		[HttpDelete("{commentId}"), Authorize]
		public async Task<ActionResult> DeleteComment([FromRoute] int commentId)
		{
			try
			{
				await _commentsService.DeleteComment(commentId);
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

		[HttpDelete("remove/{commentId}"), Authorize(Roles = "Admin")]
		public async Task<ActionResult> RemoveCommentByModeration([FromRoute] int commentId)
		{
			try
			{
				await _commentsService.ModerationRemoveComment(commentId);
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
