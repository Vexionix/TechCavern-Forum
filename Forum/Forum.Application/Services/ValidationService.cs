﻿using Forum.Core.Entities;
using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Models;

namespace Forum.Application.Services
{
	public class ValidationService : IValidationService
	{
		private readonly IUserRepository _userRepository;
		private readonly IPasswordService _passwordService;
		public ValidationService(IUserRepository userRepository, IPasswordService passwordService)
		{
			_userRepository = userRepository;
			_passwordService = passwordService;
		}

		public async Task CheckRegisterConditions(UserRegister userRegisterBody)
		{
			char[] allowedCharsForUsername = ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWQXYZ1234567890_").ToCharArray();

			if (userRegisterBody.Username.Length < 4 || userRegisterBody.Username.Length > 16 || userRegisterBody.Username.Any(ch => !allowedCharsForUsername.Contains(ch)))
			{
				throw new BadRequestException("Username does not meet criteria.");
			}

			if (await _userRepository.UserAlreadyExists(userRegisterBody.Username, userRegisterBody.Email) == true)
			{
				throw new BadRequestException("User with the same username or email already exists");
			}

			if (!_passwordService.IsCompliantWithValidityCriteria(userRegisterBody.Password))
			{
				throw new BadRequestException("Password does not meet criteria.");
			}
		}

		public void CheckLoginConditions(User user, UserLogin userLoginBody)
		{
			if (user is null)
			{
				throw new BadRequestException("No user with such username exists.");
			}

			if (!_passwordService.IsMatchingHash(userLoginBody.Password, user.PasswordHash))
			{
				throw new BadRequestException("Wrong password.");
			}
		}
	}
}
