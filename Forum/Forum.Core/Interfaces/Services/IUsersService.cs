using Forum.Core.Entities;
using Forum.Models;
namespace Forum.Core.Interfaces.Services
{
	public interface IUsersService
	{
		Task<User> GetUserById(int userId);
		Task<string> GetUsernameForUser(int userId);
		Task<int> GetActiveUsersNumber();
	}
}
