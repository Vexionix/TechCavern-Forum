using Forum.Core.Entities;
using Forum.Models;
namespace Forum.Core.Interfaces.Services
{
	public interface ICommentsService
	{
		Task<Comment> GetCommentById(int commentId);
		Task<List<Comment>> GetCommentsForPost(int postId);
		Task AddComment(CommentCreateDto comment);
		Task EditComment(int commentId, CommentEditDto comment);
		Task DeleteComment(int commentId);
		Task ModerationRemoveComment(int commentId);
	}
}
