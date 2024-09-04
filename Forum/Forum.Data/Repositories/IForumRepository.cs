using Forum.Core.Entities;
using Forum.Data.Repositories;

namespace Forum.Data.Repositories
{
	internal interface IForumRepository
	{
		User GetUserById(int id);
		IEnumerable<User> GetAllUsers();
		void AddUser(User user);


		IEnumerable<Title> GetTitlesForUser(int userId);
		void UnlockTitleForUser(int userId, int titleId);


		IEnumerable<Category> GetAllCategories();


		IEnumerable<Subcategory> GetSubCategoriesForCategory(int categoryId);


		IEnumerable<Post> GetPostsForSubcategory(int subcategoryId);
		Post GetLatestPostForSubcategory(int subcategoryId);
		void AddPost(Post post);
		void UpdatePost(Post post);
		void DeletePost(Post post);
		void RemovePost(Post post);


		IEnumerable<Comment> GetCommentsForPost(int postId);
		IEnumerable<Comment> GetCommentsForUser(int userId);
		void AddComment(Comment comment);
		void UpdateComment(Comment comment);
		void DeleteComment(Comment comment);
		void RemoveComment(Comment comment);
	}
}
