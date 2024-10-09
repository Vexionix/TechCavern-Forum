using Forum.API.Services;
using Forum.Core.Entities;
using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Core.Models;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUsersService _usersService;

        public UsersController(IUserRepository userRepository, IUsersService usersService)
        {
            _userRepository = userRepository;
            _usersService = usersService;
        }

        [HttpGet("{userId}"), Authorize]
        public async Task<ActionResult<User>> GetUserById([FromRoute] int userId)
        {
            try
            {
                User user = await _usersService.GetUserById(userId);
                return StatusCode(StatusCodes.Status200OK, user);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
            }
        }

        [HttpGet("{userId}/profile-data"), Authorize]
        public async Task<ActionResult<GetUserProfileData>> GetUserProfileById([FromRoute] int userId)
        {
            try
            {
                GetUserProfileData userProfile = await _usersService.GetUserProfileById(userId);
                return StatusCode(StatusCodes.Status200OK, userProfile);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
            }
        }

        [HttpGet("{userId}/username"), Authorize]
        public async Task<ActionResult<string>> GetUsernameForUserById([FromRoute] int userId)
        {
            try
            {
                string username = await _usersService.GetUsernameForUser(userId);
                return StatusCode(StatusCodes.Status200OK, username);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
            }
        }

        [HttpGet("staff"), Authorize]
        public async Task<ActionResult<List<StaffGetDto>>> GetStaff()
        {
            try
            {
                List<StaffGetDto> staff = await _usersService.GetStaff();
                return StatusCode(StatusCodes.Status200OK, staff);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
            }
        }

        [HttpGet("titles/{userId}"), Authorize]
        public async Task<ActionResult<List<string>>> GetTitlesForUser([FromRoute] int userId)
        {
            try
            {
                List<string> titles = await _usersService.GetTitlesForUser(userId);
                return StatusCode(StatusCodes.Status200OK, titles);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
            }
        }

        [HttpGet("active"), Authorize]
        public async Task<ActionResult<int>> GetActiveUsers()
        {
            try
            {
                int activeUsers = await _usersService.GetActiveUsersNumber();
                return StatusCode(StatusCodes.Status200OK, activeUsers);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
            }
        }

        [HttpPatch("{userId}/activity"), Authorize]
        public async Task<ActionResult> UpdateActiveStatus([FromRoute] int userId, [FromBody] UserStatusDto userStatusModel)
        {
            try
            {
                await _usersService.UpdateActiveStatus(userId, userStatusModel.IsActive);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
            }
        }

        [HttpPut("{userId}/update-profile"), Authorize]
        public async Task<ActionResult> EditUserProfile([FromRoute] int userId, [FromBody] UserProfileEditDto userProfileEditModel)
        {
            try
            {
                await _usersService.EditUserProfile(userId, userProfileEditModel);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
            }
        }

        [HttpPatch("{userId}/ban"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> BanUser([FromRoute] int userId)
        {
            try
            {
                await _usersService.UpdateUserBanStatus(userId, true);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
            }
        }

        [HttpPatch("{userId}/unban"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> UnbanUser([FromRoute] int userId)
        {
            try
            {
                await _usersService.UpdateUserBanStatus(userId, false);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
            }
        }
    }
}
