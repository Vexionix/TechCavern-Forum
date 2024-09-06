using Forum.Core.Entities;
using Forum.Data.Repositories;

namespace Forum.Data.Repositories
{
	public interface IForumRepository
	{
		Task<User> GetUserById(int id);
		Task<IEnumerable<User>> GetAllUsers();
		Task AddUser(User user);
		Task<bool> DoesUserWithUsernameOrEmailExist(string username, string email);


		Task<IEnumerable<Title>> GetTitlesForUser(int userId);
		Task UnlockTitleForUser(int userId, int titleId);


		Task<IEnumerable<Category>> GetAllCategories();


		Task<IEnumerable<Subcategory>> GetSubCategoriesForCategory(int categoryId);


		Task<IEnumerable<Post>> GetPostsForSubcategory(int subcategoryId);
		Task<Post> GetPostById(int id);
		Task<Post> GetLatestPostForSubcategory(int subcategoryId);
		Task AddPost(Post post);
		Task EditPost(Post post);
		Task DeletePost(int postId);
		Task RemovePost(int postId);


		Task<IEnumerable<Comment>> GetCommentsForPost(int postId);
		Task<IEnumerable<Comment>> GetCommentsForUser(int userId);
		Task AddComment(Comment comment);
		Task EditComment(Comment comment);
		Task DeleteComment(int commentId);
		Task RemoveComment(int commentId);
	}
}
