using Forum.Core.Entities;
using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Models;

namespace Forum.API.Services
{
	public class UsersService : IUsersService
	{
		private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        public UsersService(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository) 
		{
			_userRepository = userRepository;
			_postRepository = postRepository;
			_commentRepository = commentRepository;
		}

		public async Task<User> GetUserById(int userId)
		{
			User? user = await _userRepository.GetUserById(userId);

			if (user == null)
			{
				throw new BadRequestException("No post with such id exists.");
			}

			return user;
		}

        public async Task<GetUserProfileData> GetUserProfileById(int userId)
        {
            User? user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                throw new BadRequestException("No post with such id exists.");
            }

            return new GetUserProfileData() { Id = user.Id, Bio = user.Bio, CreatedAt = user.CreatedAt, IsActive = user.IsActive, IsBanned = user.IsBanned, LastSeenOn = user.LastLoggedIn, Role = user.Role == Core.Enums.Role.Admin ? "Admin" : "Member", SelectedTitle = user.SelectedTitle, Username = user.Username, TotalPosts = await _postRepository.GetPostCountForUser(userId), TotalComments = await _commentRepository.GetCommentCountForUser(userId) };
        }

		public async Task<List<string>> GetTitlesForUser(int userId)
		{
			return (await _userRepository.GetTitlesForUser(userId)).ToList();
		}

        public async Task<List<StaffGetDto>> GetStaff()
        {
			IEnumerable<User> users = await _userRepository.GetStaff();

			return users.Select(u => new StaffGetDto() { Id = u.Id, CreatedAt = u.CreatedAt, IsActive = u.IsActive, LastSeenOn = u.LastLoggedIn, Role = u.Role == Core.Enums.Role.Admin ? "Admin" : "Retired", SelectedTitle = u.SelectedTitle, Username = u.Username, Bio = u.Bio}).ToList();
        }

        public async Task<string> GetUsernameForUser(int userId)
		{
			return await _userRepository.GetUsernameById(userId);
		}

		public async Task EditUserProfile(int userId, UserProfileEditDto userProfileEditModel)
		{
			await _userRepository.EditUserProfile(userId, userProfileEditModel);
		}

        public async Task<int> GetActiveUsersNumber()
		{
			return await _userRepository.GetActiveUsersNumber();
		}

        public async Task UnlockTitleForUser(int userId, int titleId)
        {
            await _userRepository.UnlockTitleForUser(userId, titleId);
        }

        public async Task UpdateActiveStatus(int userId, bool status)
		{
			await _userRepository.UpdateActiveStatus(userId, status);
		}

        public async Task UpdateUserBanStatus(int userId, bool status)
        {
            await _userRepository.UpdateBanStatus(userId, status);
        }
    }
}