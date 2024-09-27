using Forum.Core.Entities;

namespace Forum.Core.Interfaces.Repositories
{
	public interface ICommentRepository
	{
		Task<Comment?> GetCommentById(int commentId);
		Task<IEnumerable<Comment>> GetCommentsForPost(int postId);
		Task<IEnumerable<Comment>> GetCommentsForUser(int userId);
		Task AddComment(Comment comment);
		Task EditComment(Comment comment);
		Task DeleteComment(int commentId);
		Task RemoveComment(int commentId);
	}
}
