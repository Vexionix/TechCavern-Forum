using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Services;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SubcategoriesController : ControllerBase
	{
		private readonly ICategoriesService _categoriesService;

		public SubcategoriesController(ICategoriesService categoriesService)
		{
            _categoriesService = categoriesService;
		}

		[HttpGet("{subcategoryId}/max-pages"), Authorize]
		public async Task<ActionResult<List<CategoryGetDto>>> GetMaxPagesForSubcategory([FromRoute] int subcategoryId)
		{
			try
			{
				int maxPagesCount = await _categoriesService.GetMaxPagesForSubcategory(subcategoryId, 10);
				return StatusCode(StatusCodes.Status200OK, maxPagesCount);

			}
			catch (BadRequestException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occured.");
			}
		}
	}
}
