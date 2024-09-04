using Forum.Core.Entities;

namespace Forum.Data.Repositories
{
	public class ForumRepository : IForumRepository
	{
		public User GetUserById(int id)
		{
			throw new NotImplementedException();
		}
		public IEnumerable<User> GetAllUsers()
		{
			throw new NotImplementedException();
		}
		public void AddUser(User user)
		{
			throw new NotImplementedException();
		}


		public IEnumerable<Title> GetTitlesForUser(int userId)
		{
			throw new NotImplementedException();
		}
		public void UnlockTitleForUser(int userId, int titleId)
		{
			throw new NotImplementedException();
		}


		public IEnumerable<Category> GetAllCategories()
		{
			throw new NotImplementedException();
		}


		public IEnumerable<Subcategory> GetSubCategoriesForCategory(int categoryId)
		{
			throw new NotImplementedException();
		}


		public Post GetLatestPostForSubcategory(int subcategoryId)
		{
			throw new NotImplementedException();
		}
		public IEnumerable<Post> GetPostsForSubcategory(int subcategoryId)
		{
			throw new NotImplementedException();
		}
		public void AddPost(Post post)
		{
			throw new NotImplementedException();
		}
		public void UpdatePost(Post post)
		{
			throw new NotImplementedException();
		}
		public void DeletePost(Post post)
		{
			throw new NotImplementedException();
		}
		public void RemovePost(Post post)
		{
			throw new NotImplementedException();
		}


		public IEnumerable<Comment> GetCommentsForPost(int postId)
		{
			throw new NotImplementedException();
		}
		public IEnumerable<Comment> GetCommentsForUser(int userId)
		{
			throw new NotImplementedException();
		}
		public void AddComment(Comment comment)
		{
			throw new NotImplementedException();
		}
		public void UpdateComment(Comment comment)
		{
			throw new NotImplementedException();
		}
		public void DeleteComment(Comment comment)
		{
			throw new NotImplementedException();
		}
		public void RemoveComment(Comment comment)
		{
			throw new NotImplementedException();
		}
	}

}
