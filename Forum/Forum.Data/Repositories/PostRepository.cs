using Forum.Core.Entities;
using Forum.Core.Interfaces.Repositories;
using Forum.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data.Repositories
{
	public class PostRepository : IPostRepository
	{
		private readonly ForumDbContext _forumDbContext;

		public PostRepository(ForumDbContext forumDbContext)
		{
			_forumDbContext = forumDbContext;
		}

		public async Task<(IEnumerable<Post> pinnedPosts, IEnumerable<Post> regularPosts)> GetPostsForSubcategory(int subcategoryId, int page = 1, int pageSize = 10)
		{
            var pinnedPosts = await _forumDbContext.Posts
                .Where(x => x.SubcategoryId == subcategoryId && x.IsPinned)
                .Include(p => p.User)
                .OrderByDescending(x => x.LatestCommentDate ?? x.CreatedAt)
                .ToListAsync();

            var regularPosts = await _forumDbContext.Posts
                .Where(x => x.SubcategoryId == subcategoryId && !x.IsPinned)
                .Include(p => p.User)
                .OrderByDescending(x => x.LatestCommentDate ?? x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (pinnedPosts, regularPosts);
        }

        public async Task<IEnumerable<Post>> GetLatestPosts()
        {
            return await _forumDbContext.Posts
				.Include(p => p.User)
				.OrderByDescending(x => x.LatestCommentDate ?? x.CreatedAt)
                .Take(5)
                .Select(y => new Post(y.Title, y.Content, y.UserId, y.SubcategoryId) { Id = y.Id, CreatedAt = y.CreatedAt, LatestCommentDate = y.LatestCommentDate, LatestCommentId = y.LatestCommentId, User = y.User, IsDeleted = y.IsDeleted, IsRemovedByAdmin = y.IsRemovedByAdmin })
                .ToListAsync();
        }

        public async Task<Post?> GetLatestPostForSubcategory(int subcategoryId)
		{
			return await _forumDbContext.Posts
				.OrderByDescending(x => x.LatestCommentDate ?? x.CreatedAt)
                .FirstOrDefaultAsync(x => x.Subcategory.Id == subcategoryId);
		}

		public async Task<Post?> GetPostById(int id)
		{
			return await _forumDbContext.Posts
                .Where(p => p.Id == id)
                .Include(p => p.User)
				.Select(p => new Post(p.Title, p.Content, p.UserId, p.SubcategoryId ) { Id = id, User = p.User, LatestCommentDate = p.LatestCommentDate, LastEditedAt = p.LastEditedAt, CreatedAt = p.CreatedAt, IsDeleted = p.IsDeleted, IsEdited = p.IsEdited, IsLocked = p.IsLocked, IsPinned = p.IsPinned, IsRemovedByAdmin = p.IsRemovedByAdmin, LatestCommentId = p.LatestCommentId, NumberOfLikes = p.NumberOfLikes, NumberOfViews = p.NumberOfViews  })
				.FirstOrDefaultAsync(x => x.Id == id);
		}

        public async Task<int> GetCommentCount(int postId)
		{
			return await _forumDbContext.Comments
				.Where(c => c.PostId == postId)
				.CountAsync();
		}

        public async Task<int> GetTotalPostsNumber()
        {
            return await _forumDbContext.Posts.CountAsync();
        }

        public async Task<int> GetPostsAddedTodayNumber()
		{
			DateTime now = DateTime.UtcNow;
			DateTime yesterday = now.AddDays(-1);
			return await _forumDbContext.Posts.Where(x => x.CreatedAt > yesterday && x.CreatedAt <= now ).CountAsync();
		}

		public async Task IncrementPostViews(int id)
		{
            Post? postToEdit = await _forumDbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (postToEdit is not null)
            {
                postToEdit.NumberOfViews++;
                await _forumDbContext.SaveChangesAsync();
            }
        }

        public async Task AddPost(Post post)
		{
			await _forumDbContext.Posts.AddAsync(post);
			await _forumDbContext.SaveChangesAsync();
		}

		public async Task EditPost(Post editedPost)
		{
			Post? postToEdit = await _forumDbContext.Posts.FirstOrDefaultAsync(x => x.Id == editedPost.Id);

			if (postToEdit is not null)
			{
				postToEdit.Title = editedPost.Title;
				postToEdit.Content = editedPost.Content;
				postToEdit.IsEdited = true;
				postToEdit.LastEditedAt = DateTime.UtcNow;
				await _forumDbContext.SaveChangesAsync();
			}
		}

		public async Task DeletePost(int postId)
		{
			Post? postToDelete = await _forumDbContext.Posts.FirstOrDefaultAsync(x => x.Id == postId);

			if (postToDelete is not null)
			{
				postToDelete.IsDeleted = true;
				postToDelete.Content = "Post has been deleted by the user.";
				postToDelete.IsEdited = false;
				postToDelete.IsLocked = true;
				await _forumDbContext.SaveChangesAsync();
			}
		}

		public async Task RemovePost(int postId)
		{
			Post? postToRemove = await _forumDbContext.Posts.FirstOrDefaultAsync(x => x.Id == postId);

			if (postToRemove is not null)
			{
				postToRemove.IsRemovedByAdmin = true;
				postToRemove.Content = "Post has been removed by the moderation team.";
				postToRemove.IsEdited = false;
				postToRemove.IsLocked = true;
				await _forumDbContext.SaveChangesAsync();
			}
		}

		public async Task UpdatePinStatus(int postId)
		{
			Post? postToEdit = await _forumDbContext.Posts.FirstOrDefaultAsync(x => x.Id == postId);

			if (postToEdit is not null)
			{
				postToEdit.IsPinned = !postToEdit.IsPinned;
				await _forumDbContext.SaveChangesAsync();
			}
		}

		public async Task UpdateLockStatus(int postId)
		{
			Post? postToEdit = await _forumDbContext.Posts.FirstOrDefaultAsync(x => x.Id == postId);

			if (postToEdit is not null)
			{
				postToEdit.IsLocked = !postToEdit.IsLocked;
				await _forumDbContext.SaveChangesAsync();
			}
		}
	}
}
