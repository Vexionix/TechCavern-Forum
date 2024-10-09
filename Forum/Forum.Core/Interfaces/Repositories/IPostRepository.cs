using Forum.Core.Entities;

namespace Forum.Core.Interfaces.Repositories
{
	public interface IPostRepository
	{
		Task<(IEnumerable<Post> pinnedPosts, IEnumerable<Post> regularPosts)> GetPostsForSubcategory(int subcategoryId, int page = 1, int pageSize = 10);

        Task<IEnumerable<Post>> GetLatestPosts();
        Task<Post?> GetPostById(int id);
        Task<IEnumerable<Post>> GetLatestPostsForUser(int userId);
        Task<Post?> GetLatestPostForSubcategory(int subcategoryId);
        Task<int> GetCommentCount(int postId);
        Task<int> GetPostCountForUser(int userId);
        Task<int> GetTotalPostsNumber();
        Task<int> GetPostsAddedTodayNumber();
        Task IncrementPostViews(int id);
        Task AddPost(Post post);
		Task EditPost(Post post);
		Task DeletePost(int postId);
		Task RemovePost(int postId);
		Task UpdatePinStatus(int postId);
		Task UpdateLockStatus(int postId);

	}
}
