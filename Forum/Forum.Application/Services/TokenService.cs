using Forum.API.Models;
using Forum.Core.Entities;
using Forum.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Forum.Application.Services
{
	public class TokenService : ITokenService
	{
		public RefreshTokenDto GenerateRefreshToken()
		{
			var refreshToken = new RefreshTokenDto
			{
				Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
				ExpiresAt = DateTime.Now.AddDays(7)
			};

			return refreshToken;
		}

		public void SetRefreshToken(RefreshTokenDto refreshToken, HttpResponse response)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = refreshToken.ExpiresAt
			};

			response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
		}

		public string CreateToken(User user, string privateKey)
		{
			if (user is null)
			{
				return "";
			}

			string role;

			switch (user.Role)
			{
				case Core.Enums.Role.Member:
					role = "Member";
					break;
				case Core.Enums.Role.Admin:
					role = "Admin";
					break;
				default:
					role = "defaultValue";
					break;
			}

			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Role, role)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey!));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddMinutes(15),
				signingCredentials: creds
				);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}

		public ClaimsPrincipal? ValidateExpiredToken(string token, string privateKey)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(privateKey!);

			try
			{
				var validationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false, 
					ValidateLifetime = false, 
					ValidateIssuerSigningKey = true, 
					IssuerSigningKey = new SymmetricSecurityKey(key),
				};

				var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

				var jwtToken = validatedToken as JwtSecurityToken;
				if (jwtToken == null)
				{
					return null; 
				}

				var expirationClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
				if (expirationClaim != null)
				{
					var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim)).UtcDateTime;

					if (expirationTime < DateTime.UtcNow)
					{
						return principal; 
					}
					else
					{
						return principal;
					}
				}

				return null;
			}
			catch (SecurityTokenException)
			{
				return null;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
