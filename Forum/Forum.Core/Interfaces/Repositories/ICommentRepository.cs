using Forum.Core.Entities;

namespace Forum.Core.Interfaces.Repositories
{
	public interface ICommentRepository
	{
		Task<Comment?> GetCommentById(int commentId);
		Task<IEnumerable<Comment>> GetLatestCommentsForUser(int userId);
        Task<int> GetCommentCountForPost(int postId);
		Task<int> GetCommentCountForUser(int userId);
        Task<int> GetTotalCommentsNumber();
        Task<IEnumerable<Comment>> GetCommentsForPost(int postId, int page = 1, int pageSize = 10);
		Task<IEnumerable<Comment>> GetCommentsForUser(int userId);
		Task AddComment(Comment comment);
		Task EditComment(Comment comment);
		Task DeleteComment(int commentId);
		Task RemoveComment(int commentId);
	}
}
