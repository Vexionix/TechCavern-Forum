using Forum.Core.Entities;
using Forum.Models;
namespace Forum.Core.Interfaces.Services
{
	public interface IUsersService
	{
        Task<User> GetUserById(int userId);
        Task<GetUserProfileData> GetUserProfileById(int userId);
        Task<List<string>> GetTitlesForUser(int userId);
        Task<List<StaffGetDto>> GetStaff();
        Task<string> GetUsernameForUser(int userId);
		Task<int> GetActiveUsersNumber();
        Task EditUserProfile(int userId, UserProfileEditDto userProfileEditModel);
        Task UnlockTitleForUser(int userId, int titleId);
        Task UpdateActiveStatus(int userId, bool status);
        Task UpdateUserBanStatus(int userId, bool status);
    }
}
