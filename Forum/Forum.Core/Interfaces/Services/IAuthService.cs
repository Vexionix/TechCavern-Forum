using Forum.Models;
using Microsoft.AspNetCore.Http;

namespace Forum.Core.Interfaces.Services
{
	public interface IAuthService
	{
		Task RegisterUser(UserRegister userRegisterBody);
		Task<string> LoginUser(UserLogin userLoginBody, string privateKey, HttpResponse response);
		Task<string> RefreshToken(string refreshToken, int userId, string privateKey, HttpResponse response);
	}
}
