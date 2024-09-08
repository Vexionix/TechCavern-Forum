using Forum.Core.Entities;

namespace Forum.Core.Interfaces.Repositories
{
	public interface IUserRepository
	{
		Task<User?> GetUserById(int id);
		Task<User?> GetUserByUsername(string username);
		Task<IEnumerable<User>> GetAllUsers();
		Task AddUser(User user);
		Task<bool> UserAlreadyExists(string username, string email);

		Task<IEnumerable<Title>> GetTitlesForUser(int userId);
		Task UnlockTitleForUser(int userId, int titleId);
	}
}
