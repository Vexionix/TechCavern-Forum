using Forum.Core.Entities;
using Forum.Core.Interfaces.Repositories;
using Forum.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly ForumDbContext _forumDbContext;

		public CategoryRepository(ForumDbContext forumDbContext)
		{
			_forumDbContext = forumDbContext;
		}

		public async Task<IEnumerable<Category>> GetAllCategories()
		{
			return await _forumDbContext.Categories
				.Select(x => new Category(x.Name) { Id = x.Id })
				.ToListAsync();
		}

		public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryId(int categoryId)
		{
			return await _forumDbContext.Subcategories
				.Where(x => x.CategoryId == categoryId)
				.Select(y => new Subcategory(y.Name, y.CategoryId) { Id = y.Id })
				.ToListAsync();
		}
	}
}
