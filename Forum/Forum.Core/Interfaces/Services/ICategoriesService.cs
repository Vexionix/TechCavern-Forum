using Forum.Core.Entities;
using Forum.Models;
namespace Forum.Core.Interfaces.Services
{
	public interface ICategoriesService
	{
		Task<List<CategoryGetDto>> GetCategoriesWithSubcategories();
	}
}
