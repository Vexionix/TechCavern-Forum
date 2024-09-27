using Forum.Core.Entities;
using Forum.Core.Interfaces.Repositories;
using Forum.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ForumDbContext _forumDbContext;
		private string _defaultTitleName = "Member";

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

		public async Task<string> GetUsernameById(int id)
		{
			User? user = await _forumDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
			if (user is not null)
				return user.Username;
			else return "-";
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

		public async Task<int> GetActiveUsersNumber()
		{
			return await _forumDbContext.Users.Where(x => x.IsActive == true).CountAsync();
		}

		public async Task AddUser(User user)
		{
			await _forumDbContext.Users.AddAsync(user);

			Title? defaultTitle = await GetTitleByName(_defaultTitleName);
			if (defaultTitle == null)
			{
				await AddTitle(_defaultTitleName);
				defaultTitle = await GetTitleByName(_defaultTitleName);
			}
			if (defaultTitle != null)
			{
				user.Titles.Add(defaultTitle);
				user.SelectedTitle = defaultTitle.TitleName;
			}

			await _forumDbContext.SaveChangesAsync();
		}

		public async Task<bool> UserAlreadyExists(string username, string email)
		{
			return await _forumDbContext.Users.Where(x => x.Username == username || x.Email == email).AnyAsync();
		}

		public async Task<IEnumerable<RefreshToken>> GetRefreshTokensForUserId(int userId)
		{
			return await _forumDbContext.RefreshTokens.Where(x => x.UserId == userId)
				.Select(y => new RefreshToken(y.UserId, y.Token, y.ExpiresAt) { CreatedAt = y.CreatedAt })
				.ToListAsync();
		}

		public async Task AddRefreshToken(RefreshToken token) 
		{
			User? user = await GetUserById(token.UserId);

			if(user is not null)
			{
				user.RefreshTokens.Add(token);
				await _forumDbContext.SaveChangesAsync();
			}
		}

		public async Task RemoveRefreshToken(string token)
		{
			RefreshToken? refreshToken = await _forumDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

			if(refreshToken is not null)
			{
				_forumDbContext.Remove(refreshToken);
				await _forumDbContext.SaveChangesAsync();
			}

		}

		public async Task RemoveExpiredRefreshTokens()
		{
			await _forumDbContext.RefreshTokens.Where(token => token.ExpiresAt < DateTime.Now).ExecuteDeleteAsync();

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

		public async Task<Title?> GetTitleByName(string name)
		{
			Title? title = await _forumDbContext.Titles.FirstOrDefaultAsync(x => x.TitleName == name);

			return title;
		}

		public async Task AddTitle(string name)
		{
			Title? title = await _forumDbContext.Titles.FirstOrDefaultAsync(x => x.TitleName == name);
			
			if (title is not null)
			{
				return;
			}

			await _forumDbContext.Titles.AddAsync(new Title(name));
			await _forumDbContext.SaveChangesAsync();
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
