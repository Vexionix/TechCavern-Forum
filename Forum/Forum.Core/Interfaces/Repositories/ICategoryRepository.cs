using Forum.Core.Entities;

namespace Forum.Core.Interfaces.Repositories
{
	public interface ICategoryRepository
	{
		Task<IEnumerable<Category>> GetAllCategories();
		Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryId(int categoryId);
	}
}
