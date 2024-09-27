using Forum.Core.Entities;

namespace Forum.Core.Interfaces.Repositories
{
	public interface IUserRepository
	{
		Task<User?> GetUserById(int id);
		Task<User?> GetUserByUsername(string username);
		Task<string> GetUsernameById(int id);
		Task<IEnumerable<User>> GetAllUsers();
		Task<int> GetActiveUsersNumber();
		Task AddUser(User user);
		Task<bool> UserAlreadyExists(string username, string email);
		Task<IEnumerable<RefreshToken>> GetRefreshTokensForUserId(int userId);
		Task AddRefreshToken(RefreshToken token);
		Task RemoveRefreshToken(string token);
		Task RemoveExpiredRefreshTokens();
		Task<IEnumerable<Title>> GetTitlesForUser(int userId);
		Task<Title?> GetTitleByName(string name);
		Task AddTitle(string name);
		Task UnlockTitleForUser(int userId, int titleId);
	}
}
