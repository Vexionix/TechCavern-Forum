using Forum.API.Models;
using Forum.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Forum.Core.Interfaces.Services
{
	public interface ITokenService
	{
		RefreshTokenDto GenerateRefreshToken();
		void SetRefreshToken(RefreshTokenDto refreshToken, HttpResponse response);
		string CreateToken(User user, string privateKey);
	}
}
