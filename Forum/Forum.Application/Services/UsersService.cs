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
		public UsersService(IUserRepository userRepository) 
		{
			_userRepository = userRepository;
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

		public async Task<string> GetUsernameForUser(int userId)
		{
			return await _userRepository.GetUsernameById(userId);
		}

		public async Task<int> GetActiveUsersNumber()
		{
			return await _userRepository.GetActiveUsersNumber();
		}

		public async Task UpdateActiveStatus(int userId, bool status)
		{
			await _userRepository.UpdateActiveStatus(userId, status);
		}
    }
}