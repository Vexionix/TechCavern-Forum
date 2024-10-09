using Forum.Core.Entities;
using Forum.Core.Models;
using Forum.Models;
namespace Forum.Core.Interfaces.Services
{
	public interface IPostsService
	{
		Task<GetPostPageData> GetPostById(int postId, int page = 1, int pageSize = 10);
		Task<(List<PostListElementDto> pinnedPosts, List<PostListElementDto> regularPosts)> GetPostsForSubcategory(int subcategoryId, int page = 1, int pageSize = 10);
        Task<List<GetPostDisplayDataDto>> GetLatestPosts();
		Task<List<GetPostProfile>> GetLatestPostsForUser(int userId);
        Task<int> GetMaxPagesForPost(int postId, int pageSize);
		Task IncrementPostViews(int postId);
        Task<int> GetPostsAddedToday();
		Task AddPost(PostCreateDto post);
		Task EditPost(int postId, PostEditDto post);
		Task DeletePost(int postId);
		Task ModerationRemovePost(int postId);
		Task UpdatePinStatus(int postId);
		Task UpdateLockStatus(int postId);
	}
}
