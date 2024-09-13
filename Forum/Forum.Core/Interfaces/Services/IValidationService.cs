using Forum.API.Models;
using Forum.Core.Entities;
using Forum.Models;

namespace Forum.Core.Interfaces.Services
{
	public interface IValidationService
	{
		Task CheckRegisterConditions(UserRegister userRegisterBody);
		void CheckLoginConditions(User user, UserLogin userLoginBody);
	}
}
