using Forum.Core.Entities;
using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Models;

namespace Forum.API.Services
{
	public class CommentsService : ICommentsService
	{
		private readonly ICommentRepository _commentRepository;
		public CommentsService(ICommentRepository commentRepository) 
		{
			_commentRepository = commentRepository;
		}

		public async Task<CommentGetDto> GetCommentById(int commentId)
		{
			Comment? comment = await _commentRepository.GetCommentById(commentId);

			if (comment == null)
			{
				throw new BadRequestException("No comment with such id exists.");
			}

			return new CommentGetDto() { UserId = comment.UserId, Content = comment.Content };
		}

		public async Task<List<Comment>> GetCommentsForPost(int postId)
		{
			return (await _commentRepository.GetCommentsForPost(postId)).ToList();
		}

		public async Task AddComment(CommentCreateDto commentCreateModel)
		{
			if (commentCreateModel.Content == null || commentCreateModel.PostId == 0 || commentCreateModel.UserId == 0)
				throw new BadRequestException("Invalid form data");
			await _commentRepository.AddComment(new Comment(commentCreateModel.Content, commentCreateModel.UserId, commentCreateModel.PostId));
		}

		public async Task EditComment(int commentId, CommentEditDto commentEditModel)
		{
			Comment? comment = await _commentRepository.GetCommentById(commentId);

			if (comment == null)
			{
				throw new BadRequestException("No comment with such id exists.");
			}

			comment.Content = commentEditModel.Content!;

			await _commentRepository.EditComment(comment);
		}

		public async Task DeleteComment(int commentId)
		{
			await _commentRepository.DeleteComment(commentId);
		}

		public async Task ModerationRemoveComment(int commentId)
		{
			await _commentRepository.RemoveComment(commentId);
		}
	}
}