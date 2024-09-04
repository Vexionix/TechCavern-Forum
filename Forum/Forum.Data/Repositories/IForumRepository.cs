using Forum.Core.Entities;
using Forum.Data.Repositories;

namespace Forum.Data.Repositories
{
	public interface IForumRepository
	{
		User GetUserById(int id);
		IEnumerable<User> GetAllUsers();
		void AddUser(User user);


		IEnumerable<Title> GetTitlesForUser(int userId);
		void UnlockTitleForUser(int userId, int titleId);


		IEnumerable<Category> GetAllCategories();


		IEnumerable<Subcategory> GetSubCategoriesForCategory(int categoryId);


		IEnumerable<Post> GetPostsForSubcategory(int subcategoryId);
		Post GetPostById(int id);
		Post GetLatestPostForSubcategory(int subcategoryId);
		void AddPost(Post post);
		void EditPost(Post post);
		void DeletePost(int postId);
		void RemovePost(int postId);


		IEnumerable<Comment> GetCommentsForPost(int postId);
		IEnumerable<Comment> GetCommentsForUser(int userId);
		void AddComment(Comment comment);
		void EditComment(Comment comment);
		void DeleteComment(int commentId);
		void RemoveComment(int commentId);
	}
}
