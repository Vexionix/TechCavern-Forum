using Forum.Models;
using Microsoft.AspNetCore.Http;

namespace Forum.Core.Interfaces.Services
{
	public interface IAuthService
	{
		Task RegisterUser(UserRegister userRegisterBody);
		Task<string> LoginUser(UserLogin userLoginBody, string privateKey, HttpResponse response);
		Task<string> RefreshToken(string authHeader, string refreshToken, string privateKey, HttpResponse response);
	}
}
