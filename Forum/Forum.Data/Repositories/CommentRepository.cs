using Forum.Core.Entities;
using Forum.Core.Interfaces.Repositories;
using Forum.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data.Repositories
{
	public class CommentRepository : ICommentRepository
	{
		private readonly ForumDbContext _forumDbContext;

		public CommentRepository(ForumDbContext forumDbContext)
		{
			_forumDbContext = forumDbContext;
		}

		public async Task<Comment?> GetCommentById(int commentId)
		{
			Comment? comment = await _forumDbContext.Comments
				.Include(x => x.User)
				.FirstOrDefaultAsync(x => x.Id == commentId);

			return comment;
		}

        public async Task<IEnumerable<Comment>> GetLatestCommentsForUser(int userId)
        {
            return await _forumDbContext.Comments
                .Where(c => c.UserId == userId)
                .Include(c => c.User)
				.Include(c => c.Post)
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .Select(y => new Comment(y.Content, y.UserId, y.PostId) { Id = y.Id, CreatedAt = y.CreatedAt, Post = y.Post })
                .ToListAsync();
        }

        public async Task<int> GetCommentCountForPost(int postId)
		{
			return await _forumDbContext.Comments
				.Where(c => c.PostId == postId)
				.CountAsync();
		}

        public async Task<int> GetCommentCountForUser(int userId)
        {
            return await _forumDbContext.Comments
                .Where(c => c.UserId == userId)
                .CountAsync();
        }

        public async Task<int> GetTotalCommentsNumber()
        {
            return await _forumDbContext.Comments.CountAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsForPost(int postId, int page = 1, int pageSize = 10)
		{
			return await _forumDbContext.Comments
				.Include(c => c.User)
				.Where(x => x.PostId == postId)
				.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(y => new Comment(y.Content, y.UserId, y.PostId) { Id = y.Id, User = y.User, CreatedAt = y.CreatedAt, IsEdited = y.IsEdited, LastEditedAt = y.LastEditedAt, IsDeleted = y.IsDeleted, IsRemovedByAdmin = y.IsRemovedByAdmin })
				.ToListAsync();
		}

		public async Task<IEnumerable<Comment>> GetCommentsForUser(int userId)
		{
			return await _forumDbContext.Comments.Where(x => x.UserId == userId)
				.Select(y => new Comment(y.Content, y.UserId, y.PostId) { Id = y.Id })
				.ToListAsync();
		}

		public async Task AddComment(Comment comment)
		{
			await _forumDbContext.Comments.AddAsync(comment);

            await _forumDbContext.SaveChangesAsync();

            Post? postToEdit = await _forumDbContext.Posts.FirstOrDefaultAsync(x => x.Id == comment.PostId);

            if (postToEdit is not null)
            {
				postToEdit.LatestCommentId = comment.Id;
				postToEdit.LatestCommentDate = comment.CreatedAt;
            }

            await _forumDbContext.SaveChangesAsync();
		}

		public async Task EditComment(Comment editedComment)
		{
			Comment? commentToEdit = await _forumDbContext.Comments.FirstOrDefaultAsync(x => x.Id == editedComment.Id);

			if (commentToEdit is not null)
			{
				commentToEdit.Content = editedComment.Content;
				commentToEdit.IsEdited = true;
				commentToEdit.LastEditedAt = DateTime.UtcNow;
				await _forumDbContext.SaveChangesAsync();
			}
		}

		public async Task DeleteComment(int commentId)
		{
			Comment? commentToDelete = await _forumDbContext.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

			if (commentToDelete is not null)
			{
				commentToDelete.IsDeleted = true;
				commentToDelete.Content = "Comment deleted by the user.";
				commentToDelete.IsEdited = false;
				await _forumDbContext.SaveChangesAsync();
			}
		}

		public async Task RemoveComment(int commentId)
		{
			Comment? commentToRemove = await _forumDbContext.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

			if (commentToRemove is not null)
			{
				commentToRemove.IsRemovedByAdmin = true;
				commentToRemove.Content = "Comment removed by the moderation team.";
				commentToRemove.IsEdited = false;
				await _forumDbContext.SaveChangesAsync();
			}
		}
	}
}
