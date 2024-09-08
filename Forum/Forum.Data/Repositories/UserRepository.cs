using Forum.Core.Entities;
using Forum.Core.Interfaces.Repositories;
using Forum.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ForumDbContext _forumDbContext;

		public UserRepository(ForumDbContext forumDbContext)
		{
			_forumDbContext = forumDbContext;
		}

		public async Task<User?> GetUserById(int id)
		{
			return await _forumDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
		}
		public async Task<User?> GetUserByUsername(string username)
		{
			return await _forumDbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
		}
		public async Task<IEnumerable<User>> GetAllUsers()
		{
			return await _forumDbContext.Users.Include(x => x.Titles).Select(user =>
					new User(user.Username, user.Email, user.PasswordHash, user.SelectedTitle, user.Bio, user.Location)
					{
						Id = user.Id,
						CreatedAt = user.CreatedAt,
						Role = user.Role,
						Titles = user.Titles
					}
				)
				.ToListAsync();

		}
		public async Task AddUser(User user)
		{
			await _forumDbContext.Users.AddAsync(user);

			Title? defaultTitle = await _forumDbContext.Titles.FirstOrDefaultAsync(x => x.Id == 1);
			if (defaultTitle != null)
			{
				user.Titles.Add(defaultTitle);
			}

			await _forumDbContext.SaveChangesAsync();
		}
		public async Task<bool> UserAlreadyExists(string username, string email)
		{
			return await _forumDbContext.Users.Where(x => x.Username == username || x.Email == email).AnyAsync();
		}

		public async Task<IEnumerable<Title>> GetTitlesForUser(int userId)
		{
			User? user = await _forumDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

			if (user is not null)
				return await _forumDbContext.Titles
					.Where(x => x.Users
					.Contains(user))
					.Select(t => new Title(t.TitleName) { Id = t.Id })
					.ToListAsync();

			return [];
		}
		public async Task UnlockTitleForUser(int userId, int titleId)
		{
			User? user = await _forumDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
			Title? title = await _forumDbContext.Titles.FirstOrDefaultAsync(x => x.Id == titleId);

			if (user is not null && title is not null)
			{
				user.Titles.Add(title);
			}

			await _forumDbContext.SaveChangesAsync();
		}
	}
}
