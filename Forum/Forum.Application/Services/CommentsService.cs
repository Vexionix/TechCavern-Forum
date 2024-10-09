using Forum.Core.Entities;
using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Models;

namespace Forum.API.Services
{
	public class CommentsService : ICommentsService
	{
		private readonly IUserRepository _userRepository;
		private readonly ICommentRepository _commentRepository;
		public CommentsService(IUserRepository userRepository, ICommentRepository commentRepository)
        {
            _userRepository = userRepository;
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

        public async Task<List<GetCommentProfile>> GetLatestCommentsForUser(int userId)
        {
            List<Comment> comments = (await _commentRepository.GetLatestCommentsForUser(userId)).ToList();
            List<GetCommentProfile> result = new List<GetCommentProfile>();

            foreach (Comment comment in comments)
            {
                result.Add(new GetCommentProfile
                {
                    Id = comment.Id,
                    CreatedAt = comment.CreatedAt,
                    Title = comment.Post.Title,
                    PostId = comment.PostId,
					Content = comment.Content
                });
            }

            return result;
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
            
			int CommentCountForUser = await _commentRepository.GetCommentCountForUser(commentCreateModel.UserId);
            if (CommentCountForUser >= 10)
            {
                await _userRepository.UnlockTitleForUser(commentCreateModel.UserId, (await _userRepository.GetTitleByName("Commenter"))!.Id);
            }
            if (CommentCountForUser >= 25)
            {
                await _userRepository.UnlockTitleForUser(commentCreateModel.UserId, (await _userRepository.GetTitleByName("Made up opinions"))!.Id);
            }
			if (CommentCountForUser >= 50)
            {
                await _userRepository.UnlockTitleForUser(commentCreateModel.UserId, (await _userRepository.GetTitleByName("Akshually"))!.Id);
            }
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