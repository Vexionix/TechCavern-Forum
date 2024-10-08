using Forum.Core.Entities;
using Forum.Models;
namespace Forum.Core.Interfaces.Services
{
	public interface ICategoriesService
	{
		Task<int> GetMaxPagesForSubcategory(int subcategoryId, int pageSize);
        Task<List<CategoryGetDto>> GetCategoriesWithSubcategories();
	}
}
