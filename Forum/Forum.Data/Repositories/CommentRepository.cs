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

		public async Task<IEnumerable<Comment>> GetCommentsForPost(int postId)
		{
			return await _forumDbContext.Comments.Where(x => x.PostId == postId)
				.Select(y => new Comment(y.Content, y.UserId, y.PostId) { Id = y.Id })
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
		}

		public async Task EditComment(Comment editedComment)
		{
			Comment? commentToEdit = await _forumDbContext.Comments.FirstOrDefaultAsync(x => x.Id == editedComment.Id);

			if (commentToEdit is not null)
			{
				commentToEdit.Content = editedComment.Content;
				commentToEdit.IsEdited = true;
				commentToEdit.LastEditedAt = DateTime.Now;
				await _forumDbContext.SaveChangesAsync();
			}
		}

		public async Task DeleteComment(int commentId)
		{
			Comment? commentToDelete = await _forumDbContext.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

			if (commentToDelete is not null)
			{
				commentToDelete.IsRemovedByAdmin = true;
				commentToDelete.Content = "Comment removed by the moderation team.";
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
