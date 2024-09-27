using Forum.Core.Entities;
using Forum.Models;
namespace Forum.Core.Interfaces.Services
{
	public interface IPostsService
	{
		Task<Post> GetPostById(int postId);
		Task<List<Post>> GetPostsForSubcategory(int subcategoryId);
		Task<int> GetPostsAddedToday();
		Task AddPost(PostCreateDto post);
		Task EditPost(int postId, PostEditDto post);
		Task DeletePost(int postId);
		Task ModerationRemovePost(int postId);
		Task UpdatePinStatus(int postId);
		Task UpdateLockStatus(int postId);
	}
}
