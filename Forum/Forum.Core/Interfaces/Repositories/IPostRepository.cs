using Forum.Core.Entities;

namespace Forum.Core.Interfaces.Repositories
{
	public interface IPostRepository
	{
		Task<IEnumerable<Post>> GetPostsForSubcategory(int subcategoryId);
		Task<Post?> GetPostById(int id);
		Task<Post?> GetLatestPostForSubcategory(int subcategoryId);
		Task<int> GetPostsAddedToday();
		Task AddPost(Post post);
		Task EditPost(Post post);
		Task DeletePost(int postId);
		Task RemovePost(int postId);
		Task UpdatePinStatus(int postId);
		Task UpdateLockStatus(int postId);

	}
}
