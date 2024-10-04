using Forum.Core.Entities;
using Forum.Core.Models;

namespace Forum.Core.Interfaces.Repositories
{
	public interface ICategoryRepository
	{
		Task<IEnumerable<Category>> GetAllCategories();
		Task<IEnumerable<Category>> GetCategoriesWithSubcategories();

        Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryId(int categoryId);
		Task<int> GetSubcategoryTotalNumberOfComments(int subcategoryId);
		Task<int> GetSubcategoryTotalNumberOfPosts(int subcategoryId);
		Task<Post?> GetSubcategoryPostWithLatestInteraction(int subcategoryId);

    }
}
