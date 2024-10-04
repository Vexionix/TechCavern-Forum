using Forum.Core.Entities;
using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Core.Models;
using Forum.Models;

namespace Forum.API.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;
        public CategoriesService(ICategoryRepository categoryRepository, ICommentRepository commentRepository)
        {
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
        }

        public async Task<List<CategoryGetDto>> GetCategoriesWithSubcategories()
        {
            List<Category> categories = (await _categoryRepository.GetCategoriesWithSubcategories()).ToList();

            if (categories is null)
            {
                throw new BadRequestException("No categories found.");
            }

            List<CategoryGetDto> result = new List<CategoryGetDto>();

            foreach (var category in categories)
            {
                
                List<SubcategoryGetDto> subcategories = new List<SubcategoryGetDto>();

                foreach (var subcategory in category.Subcategories)
                {
                    var numberOfComments = await _categoryRepository.GetSubcategoryTotalNumberOfComments(subcategory.Id);
                    var numberOfPosts = await _categoryRepository.GetSubcategoryTotalNumberOfPosts(subcategory.Id);
                    var postWithMostRecentActivity = await GetPostWithMostRecentActivityForSubcategory(subcategory.Id);

                    subcategories.Add(new SubcategoryGetDto
                    {
                        Id = subcategory.Id,
                        Name = subcategory.Name,
                        Description = subcategory.Description,
                        GiIcon = subcategory.GiIcon,
                        NumberOfComments = numberOfComments,
                        NumberOfPosts = numberOfPosts,
                        PostWithMostRecentActivity = postWithMostRecentActivity
                    });
                }

                result.Add(new CategoryGetDto()
                {
                    Id = category.Id,
                    Name = category.Name,
                    GiIcon = category.GiIcon,
                    Subcategories = subcategories
                });
            }

            return result;
        }

        private async Task<GetSubcategoryPostDto?> GetPostWithMostRecentActivityForSubcategory(int subcategoryId)
        {
            Post? post = await _categoryRepository.GetSubcategoryPostWithLatestInteraction(subcategoryId);

            if (post is null) return null;

            DateTime latestActivity;
            string username;
            int userId;


            if (post.LatestCommentDate is not null)
            {
                latestActivity = post.LatestCommentDate.Value;

                var comment = await _commentRepository.GetCommentById(post.LatestCommentId);

                username = comment!.User.Username;
                userId = comment!.UserId;

            }
            else
            {
                latestActivity = post.CreatedAt;
                username = post.User.Username;
                userId = post.UserId;
            }


            if (post != null)
            {
                return new GetSubcategoryPostDto
                {
                    Id = post.Id,
                    LatestActivityPostedAt = latestActivity,
                    Title = post.Title,
                    Username = username,
                    UserId = userId
                };
            }

            return null;
        }
    }
}