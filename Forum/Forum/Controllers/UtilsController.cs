using Forum.Core.Interfaces.Services;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;
using Forum.Core.Exceptions;

namespace Forum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UtilsController : ControllerBase
	{
		private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
		private readonly IUtilsService _utilsService;

        public UtilsController(IEmailService emailService, IConfiguration configuration, IUtilsService utilsService)
		{
            _emailService = emailService;
			_configuration = configuration;
			_utilsService = utilsService;
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

        [HttpGet("statistics")]
        public async Task<ActionResult<StatisticsDto>> GetStatistics()
        {
            try
            {
                var statistics = await _utilsService.GetStatistics();
                return StatusCode(StatusCodes.Status200OK, statistics);
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
