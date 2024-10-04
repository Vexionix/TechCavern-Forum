using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Services;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoriesService _categoriesService;

		public CategoriesController(ICategoriesService categoriesService)
		{
            _categoriesService = categoriesService;
		}

		[HttpGet("with-subcategories"), Authorize]
		public async Task<ActionResult<List<CategoryGetDto>>> GetCategoriesWithSubcategories()
		{
			try
			{
				List<CategoryGetDto> categories = await _categoriesService.GetCategoriesWithSubcategories();
				return StatusCode(StatusCodes.Status200OK, categories);

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
