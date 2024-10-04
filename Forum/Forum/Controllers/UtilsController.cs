using Forum.Core.Interfaces.Services;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Forum.Core.Exceptions;
using Newtonsoft.Json;

namespace Forum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UtilsController : ControllerBase
	{
		private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UtilsController(IEmailService emailService, IConfiguration configuration)
		{
            _emailService = emailService;
			_configuration = configuration;
        }

		[HttpPost("contact")]
		public async Task<ActionResult> SendContactFormData([FromBody] ContactFormModel contactFormBody)
		{
			try
			{
				await _emailService.SendContactFormData(contactFormBody, _configuration.GetSection("AppSettings:EmailAppPass").Value!);
				return StatusCode(StatusCodes.Status201Created);
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
