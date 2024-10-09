using Forum.Core.Entities;
using Forum.Models;

namespace Forum.Core.Interfaces.Repositories
{
	public interface IUserRepository
	{
		Task<User?> GetUserById(int id);
        Task<User?> GetLatestUser();
        Task<User?> GetUserByUsername(string username);
		Task<string> GetUsernameById(int id);
		Task<IEnumerable<User>> GetAllUsers();
		Task<IEnumerable<User>> GetStaff();
        Task<int> GetTotalUsersNumber();
        Task<int> GetActiveUsersNumber();
        Task AddUser(User user);
        Task EditUserProfile(int userId, UserProfileEditDto userProfileEditModel);
        Task<bool> UserAlreadyExists(string username, string email);
        Task UpdateActiveStatus(int userId, bool status);
		Task UpdateBanStatus(int userId, bool status);
        Task<IEnumerable<RefreshToken>> GetRefreshTokensForUserId(int userId);
		Task AddRefreshToken(RefreshToken token);
		Task RemoveRefreshToken(string token);
		Task RemoveAllRefreshTokensForUser(int userId);
        Task RemoveExpiredRefreshTokens();
        Task<IEnumerable<string>> GetTitlesForUser(int userId);
		Task<Title?> GetTitleByName(string name);
		Task AddTitle(string name);
		Task UnlockTitleForUser(int userId, int titleId);
	}
}
