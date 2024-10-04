using Forum.Core.Entities;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Models;
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

        public async Task<IEnumerable<Category>> GetCategoriesWithSubcategories()
        {
            return await _forumDbContext.Categories.Include(x => x.Subcategories)
                .Select(x => new Category(x.Name) { Id = x.Id, Subcategories = x.Subcategories, GiIcon = x.GiIcon })
                .ToListAsync();
        }


        public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryId(int categoryId)
        {
            return await _forumDbContext.Subcategories
                .Where(x => x.CategoryId == categoryId)
                .Select(y => new Subcategory(y.Name, y.CategoryId) { Id = y.Id })
                .ToListAsync();
        }

        public async Task<int> GetSubcategoryTotalNumberOfComments(int subcategoryId)
        {
            return await _forumDbContext.Comments
                .Where(c => c.Post.SubcategoryId == subcategoryId)
                .CountAsync();
        }

        public async Task<int> GetSubcategoryTotalNumberOfPosts(int subcategoryId)
        {
            return await _forumDbContext.Posts
                .Where(p => p.SubcategoryId == subcategoryId)
                .CountAsync();
        }

        public async Task<Post?> GetSubcategoryPostWithLatestInteraction(int subcategoryId)
        {
            var post = await _forumDbContext.Posts
                .Where(s => s.SubcategoryId == subcategoryId)
                .Include(p => p.User)
                .OrderByDescending(p => p.LatestCommentDate)
                .ThenByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();

            return post;
        }
    }
}
