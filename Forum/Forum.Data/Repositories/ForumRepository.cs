using Forum.Core.Entities;
using Forum.Data.DbContexts;

namespace Forum.Data.Repositories
{
	public class ForumRepository : IForumRepository
	{
		private readonly ForumDbContext _forumDbContext;

		public ForumRepository(ForumDbContext forumDbContext) {
			_forumDbContext = forumDbContext;
		}

		// Users
		public User? GetUserById(int id)
		{
			return _forumDbContext.Users.FirstOrDefault(x => x.Id == id);
		}
		public IEnumerable<User> GetAllUsers()
		{
			return _forumDbContext.Users;
		}
		public void AddUser(User user)
		{
			_forumDbContext.Users.Add(user);
			_forumDbContext.SaveChanges();
		}


		// Titles
		public IEnumerable<Title> GetTitlesForUser(int userId)
		{
			return _forumDbContext.Titles.Where(x => x.Users.Contains(GetUserById(userId)));
		}
		public void UnlockTitleForUser(int userId, int titleId)
		{
			User? user = _forumDbContext.Users.FirstOrDefault(x => x.Id == userId);
			Title? title = _forumDbContext.Titles.FirstOrDefault(x => x.Id == titleId);
			
			if(user is not null && title is not null)
			{
				user.Titles.Add(title);
			}

			_forumDbContext.SaveChanges();
		}


		// Categories
		public IEnumerable<Category> GetAllCategories()
		{
			return _forumDbContext.Categories;
		}


		// Subcategories
		public IEnumerable<Subcategory> GetSubCategoriesForCategory(int categoryId)
		{
			return _forumDbContext.Subcategories.Where(x => x.CategoryId == categoryId);
		}


		// Posts
		public IEnumerable<Post> GetPostsForSubcategory(int subcategoryId)
		{
			return _forumDbContext.Posts.Where(x => x.SubcategoryId == subcategoryId);
		}
		public Post? GetLatestPostForSubcategory(int subcategoryId)
		{
			return _forumDbContext.Posts.OrderByDescending(x => x.CreatedAt).FirstOrDefault(x => x.Subcategory.Id == subcategoryId);
		}
		public Post? GetPostById(int id)
		{
			return _forumDbContext.Posts.FirstOrDefault(x => x.Id == id);
		}
		public void AddPost(Post post)
		{
			_forumDbContext.Posts.Add(post);
			_forumDbContext.SaveChanges();
		}
		public void EditPost(Post editedPost)
		{
			Post? postToEdit = _forumDbContext.Posts.FirstOrDefault(x => x.Id == editedPost.Id);
			
			if (postToEdit is not null)
			{
				postToEdit.Title = editedPost.Title;
				postToEdit.Content = editedPost.Content;
				postToEdit.IsEdited = true;
				postToEdit.LastEditedAt = DateTime.Now;
			}

			_forumDbContext.SaveChanges();
		}
		public void DeletePost(int postId)
		{
			Post? postToDelete = _forumDbContext.Posts.FirstOrDefault(x => x.Id == postId);

			if (postToDelete is not null)
			{
				postToDelete.IsDeleted = true;
				postToDelete.Content = "Post deleted by the user.";
				postToDelete.IsEdited = false;
			}

			_forumDbContext.SaveChanges();
		}
		public void RemovePost(int postId)
		{
			Post? PostToRemove = _forumDbContext.Posts.FirstOrDefault(x => x.Id == postId);

			if (PostToRemove is not null)
			{
				PostToRemove.IsRemovedByAdmin = true;
				PostToRemove.Content = "Post removed by the moderation team.";
				PostToRemove.IsEdited = false;
			}

			_forumDbContext.SaveChanges();
		}


		// Comments
		public IEnumerable<Comment> GetCommentsForPost(int postId)
		{
			return _forumDbContext.Comments.Where(x => x.PostId == postId);
		}
		public IEnumerable<Comment> GetCommentsForUser(int userId)
		{
			return _forumDbContext.Comments.Where(x => x.UserId == userId);
		}
		public void AddComment(Comment comment)
		{
			_forumDbContext.Comments.Add(comment);
			_forumDbContext.SaveChanges();
		}
		public void EditComment(Comment editedComment)
		{
			Comment? commentToEdit = _forumDbContext.Comments.FirstOrDefault(x => x.Id == editedComment.Id);

			if (commentToEdit is not null)
			{
				commentToEdit.Content = editedComment.Content;
				commentToEdit.IsEdited = true;
				commentToEdit.LastEditedAt = DateTime.Now;
			}

			_forumDbContext.SaveChanges();
		}
		public void DeleteComment(int commentId)
		{
			Comment? commentToDelete = _forumDbContext.Comments.FirstOrDefault(x => x.Id == commentId);

			if (commentToDelete is not null)
			{
				commentToDelete.IsRemovedByAdmin = true;
				commentToDelete.Content = "Comment removed by the moderation team.";
				commentToDelete.IsEdited = false;
			}

			_forumDbContext.SaveChanges();
		}
		public void RemoveComment(int commentId)
		{
			Comment? commentToRemove = _forumDbContext.Comments.FirstOrDefault(x => x.Id == commentId);

			if (commentToRemove is not null)
			{
				commentToRemove.IsRemovedByAdmin = true;
				commentToRemove.Content = "Comment removed by the moderation team.";
				commentToRemove.IsEdited = false;
			}

			_forumDbContext.SaveChanges();
		}
	}

}
