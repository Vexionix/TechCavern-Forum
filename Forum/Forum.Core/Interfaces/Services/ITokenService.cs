using Forum.API.Models;
using Forum.Core.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Forum.Core.Interfaces.Services
{
	public interface ITokenService
	{
		RefreshTokenDto GenerateRefreshToken();
		void SetRefreshToken(RefreshTokenDto refreshToken, HttpResponse response);
		string CreateToken(User user, string privateKey);
		ClaimsPrincipal? ValidateExpiredToken(string token, string privateKey);
	}
}
