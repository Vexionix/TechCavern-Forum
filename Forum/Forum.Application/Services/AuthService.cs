﻿using Forum.Core.Entities;
using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Models;
using Microsoft.AspNetCore.Http;

namespace Forum.API.Services
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IPasswordService _passwordService;
		private readonly IValidationService _validationService;
		private readonly ITokenService _tokenService;

		public AuthService(IUserRepository userRepository, IPasswordService passwordService, IValidationService validationService, ITokenService tokenService)
		{
			_userRepository = userRepository;
			_passwordService = passwordService;
			_validationService = validationService;
			_tokenService = tokenService;
		}

		public async Task RegisterUser(UserRegister userRegisterBody)
		{
			try
			{
				await _validationService.CheckRegisterConditions(userRegisterBody);
			}
			catch (BadRequestException ex)
			{
				throw new BadRequestException(ex.Message);
			}

			var newUser = new User(userRegisterBody.Username,
				userRegisterBody.Email,
				_passwordService.Encrypt(userRegisterBody.Password),
				"",
				"We don't know much about them, but we are sure they are cool.",
				"Romania");

			await _userRepository.AddUser(newUser);
		}

		public async Task<string> LoginUser(UserLogin userLoginBody, string privateKey, HttpResponse response)
		{
			var user = await _userRepository.GetUserByUsername(userLoginBody.Username);

			try
			{
				_validationService.CheckLoginConditions(user, userLoginBody);
			}
			catch (BadRequestException ex)
			{
				throw new BadRequestException(ex.Message);
			}

			var refreshToken = _tokenService.GenerateRefreshToken();
			_tokenService.SetRefreshToken(refreshToken, response);

			await _userRepository.AddRefreshToken(new RefreshToken(user.Id, refreshToken.Token, refreshToken.ExpiresAt)
			{
				CreatedAt = refreshToken.CreatedAt
			});

			return _tokenService.CreateToken(user, privateKey);
		}

		public async Task<string> RefreshToken(string refreshToken, int userId, string privateKey, HttpResponse response)
		{

			User? user = await _userRepository.GetUserById(userId);

			if (user is null)
			{
				throw new BadRequestException("User doesn't exist.");
			}

			if ((await _userRepository.GetRefreshTokensForUserId(userId)).Any(x => x.Token == refreshToken))
			{
				await _userRepository.RemoveRefreshToken(refreshToken!);

				string token = _tokenService.CreateToken(user, privateKey);
				var newRefreshToken = _tokenService.GenerateRefreshToken();
				_tokenService.SetRefreshToken(newRefreshToken, response);

				await _userRepository.AddRefreshToken(new RefreshToken(user.Id, newRefreshToken.Token, newRefreshToken.ExpiresAt)
				{
					CreatedAt = newRefreshToken.CreatedAt
				});

				return token;
			}

			throw new BadRequestException("Invalid refresh token");
		}
	}
}
