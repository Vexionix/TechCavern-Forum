using Forum.Core.Entities;
using Forum.Data.DbContexts;

namespace Forum.Data.Repositories
{
	public class ForumRepository : IForumRepository
	{
		private readonly ForumDbContext _forumDbContext;

		public ForumRepository(ForumDbContext forumDbContext)
		{
			_forumDbContext = forumDbContext;
		}

		// Users
		public async Task<User?> GetUserById(int id)
		{
			return _forumDbContext.Users.FirstOrDefault(x => x.Id == id);
		}
		public async Task<IEnumerable<User>> GetAllUsers()
		{
			return _forumDbContext.Users.Select(user =>
					new User(user.Username, user.Email, user.Password, user.SelectedTitle, user.Bio, user.Location)
					{
						Id = user.Id,
						CreatedAt = user.CreatedAt,
						Role = user.Role
					}
				).ToList();

		}
		public async Task AddUser(User user)
		{
			_forumDbContext.Users
				.Add(user);

			Title? defaultTitle = _forumDbContext.Titles.FirstOrDefault(x => x.Id == 1);
			if (defaultTitle != null)
			{
				user.Titles.Add(defaultTitle);
			}

			_forumDbContext.SaveChanges();
		}
		public async Task<bool> DoesUserWithUsernameOrEmailExist(string username, string email)
		{
			return _forumDbContext.Users.Where(x => x.Username ==  username || x.Email == email).Any();
		}


		// Titles
		public async Task<IEnumerable<Title>> GetTitlesForUser(int userId)
		{
			User? user = _forumDbContext.Users.FirstOrDefault(x => x.Id == userId);

			if (user is not null)
				return _forumDbContext.Titles
					.Where(x => x.Users
					.Contains(user))
					.Select(t => new Title(t.TitleName) { Id = t.Id })
					.ToList();

			return [];
		}
		public async Task UnlockTitleForUser(int userId, int titleId)
		{
			User? user = _forumDbContext.Users.FirstOrDefault(x => x.Id == userId);
			Title? title = _forumDbContext.Titles.FirstOrDefault(x => x.Id == titleId);

			if (user is not null && title is not null)
			{
				user.Titles.Add(title);
			}

			_forumDbContext.SaveChanges();
		}


		// Categories
		public async Task<IEnumerable<Category>> GetAllCategories()
		{
			return _forumDbContext.Categories.Select(x => new Category(x.Name) { Id = x.Id }).ToList();
		}


		// Subcategories
		public async Task<IEnumerable<Subcategory>> GetSubCategoriesForCategory(int categoryId)
		{
			return _forumDbContext.Subcategories.Where(x => x.CategoryId == categoryId)
				.Select(y => new Subcategory(y.Name, y.CategoryId) 
				{ 
					Id = y.Id 
				})
				.ToList();
		}


		// Posts
		public async Task<IEnumerable<Post>> GetPostsForSubcategory(int subcategoryId)
		{
			return _forumDbContext.Posts.Where(x => x.SubcategoryId == subcategoryId)
				.Select(y => new Post(y.Title, y.Content, y.UserId, y.SubcategoryId) 
				{ 
					Id = y.Id 
				})
				.ToList();
		}
		public async Task<Post?> GetLatestPostForSubcategory(int subcategoryId)
		{
			return _forumDbContext.Posts.OrderByDescending(x => x.CreatedAt).FirstOrDefault(x => x.Subcategory.Id == subcategoryId);
		}
		public async Task<Post?> GetPostById(int id)
		{
			return _forumDbContext.Posts.FirstOrDefault(x => x.Id == id);
		}
		public async Task AddPost(Post post)
		{
			_forumDbContext.Posts.Add(post);
			_forumDbContext.SaveChanges();
		}
		public async Task EditPost(Post editedPost)
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
		public async Task DeletePost(int postId)
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
		public async Task RemovePost(int postId)
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
		public async Task<IEnumerable<Comment>> GetCommentsForPost(int postId)
		{
			return _forumDbContext.Comments.Where(x => x.PostId == postId)
				.Select(y => new Comment(y.Content, y.UserId, y.PostId)
				{
					Id = y.Id
				})
				.ToList();
		}
		public async Task<IEnumerable<Comment>> GetCommentsForUser(int userId)
		{
			return _forumDbContext.Comments.Where(x => x.UserId == userId)
				.Select(y => new Comment(y.Content, y.UserId, y.PostId)
				{
					Id = y.Id
				})
				.ToList();
		}
		public async Task AddComment(Comment comment)
		{
			_forumDbContext.Comments.Add(comment);
			_forumDbContext.SaveChanges();
		}
		public async Task EditComment(Comment editedComment)
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
		public async Task DeleteComment(int commentId)
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
		public async Task RemoveComment(int commentId)
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
