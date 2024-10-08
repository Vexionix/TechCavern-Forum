using Forum.Core.Entities;
using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Core.Models;
using Forum.Models;

namespace Forum.API.Services
{
    public class PostsService : IPostsService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        public PostsService(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task<int> GetMaxPagesForPost(int postId, int pageSize)
        {
            int commentCount = await _postRepository.GetCommentCount(postId);
            return (int)Math.Ceiling((double)commentCount / pageSize);
        }

        public async Task IncrementPostViews(int postId)
        {
            await _postRepository.IncrementPostViews(postId);
        }

        public async Task<GetPostPageData> GetPostById(int postId, int page = 1, int pageSize = 10)
        {
            Post? post = await _postRepository.GetPostById(postId);
            List<Comment> comments = (await _commentRepository.GetCommentsForPost(postId, page)).ToList();

            if (post == null)
            {
                throw new BadRequestException("No post with such id exists.");
            }

            GetPostPageData getPostPageData = new GetPostPageData()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                UserId = post.UserId,
                Username = post.User.Username,
                IsPinned = post.IsPinned,
                IsLocked = post.IsLocked,
                IsEdited = post.IsEdited,
                IsDeleted = post.IsDeleted,
                IsRemoved = post.IsRemovedByAdmin,
                LastEditedAt = post.LastEditedAt,
                CreatedAt = post.CreatedAt,
                Comments = comments.Select(c => new GetCommentForPost()
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserId = c.UserId,
                    Username = c.User.Username,
                    CreatedAt = c.CreatedAt,
                    IsEdited = c.IsEdited,
                    IsDeleted = c.IsDeleted,
                    IsRemoved = c.IsRemovedByAdmin,
                    LastEditedAt = c.LastEditedAt,
                    User = new GetUserDataFoCommentAndPost() { SelectedTitle = c.User.SelectedTitle, CreatedAt = c.User.CreatedAt, IsActive = c.User.IsActive, IsBanned = c.User.IsBanned, Role = c.User.Role == 0 ? "Member" : "Admin", LastSeenOn = c.User.LastLoggedIn }
                }).ToList(),
                User = new GetUserDataFoCommentAndPost() { SelectedTitle = post.User.SelectedTitle, CreatedAt = post.User.CreatedAt, IsActive = post.User.IsActive, IsBanned = post.User.IsBanned, Role = post.User.Role == 0 ? "Member" : "Admin", LastSeenOn = post.User.LastLoggedIn }
    };

            return getPostPageData;
        }

        public async Task<(List<PostListElementDto> pinnedPosts, List<PostListElementDto> regularPosts)> GetPostsForSubcategory(int subcategoryId, int page = 1, int pageSize = 10)
        {
            (IEnumerable<Post> pinnedPosts, IEnumerable<Post> regularPosts) = await _postRepository.GetPostsForSubcategory(subcategoryId, page, pageSize);

            async Task<PostListElementDto> MapToPostListElementDto(Post p)
            {
                var latestCommentUser = p.LatestCommentDate != null
                    ? (await _commentRepository.GetCommentById(p.LatestCommentId)).User
                    : null;

                int commentCount = await _commentRepository.GetCommentCountForPost(p.Id);

                return new PostListElementDto()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Views = p.NumberOfViews,
                    CommentsCount = commentCount,
                    UserId = p.UserId,
                    LatestCommentUserId = latestCommentUser != null ? latestCommentUser.Id : 0,
                    SubcategoryId = p.SubcategoryId,
                    Username = p.User.Username,
                    LatestCommenterUsername = latestCommentUser != null ? latestCommentUser.Username : "",
                    IsPinned = p.IsPinned,
                    IsLocked = p.IsLocked,
                    CreatedAt = p.CreatedAt,
                    LatestCommentPostedAt = p.LatestCommentDate
                };
            }

            var pinnedPostsDto = new List<PostListElementDto>();
            foreach (var post in pinnedPosts)
            {
                pinnedPostsDto.Add(await MapToPostListElementDto(post));
            }

            var regularPostsDto = new List<PostListElementDto>();
            foreach (var post in regularPosts)
            {
                regularPostsDto.Add(await MapToPostListElementDto(post));
            }

            return (pinnedPostsDto, regularPostsDto);
        }

        public async Task<List<GetPostDisplayDataDto>> GetLatestPosts()
        {
            List<Post> posts = (await _postRepository.GetLatestPosts()).ToList();
            List<GetPostDisplayDataDto> result = new List<GetPostDisplayDataDto>();

            foreach (Post post in posts)
            {
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

                result.Add(new GetPostDisplayDataDto
                {
                    Id = post.Id,
                    LatestActivityPostedAt = latestActivity,
                    Title = post.Title,
                    Username = username,
                    UserId = userId
                });
            }

            return result;
        }

        public async Task<int> GetPostsAddedToday()
        {
            int postsAddedToday = await _postRepository.GetPostsAddedTodayNumber();
            return postsAddedToday;
        }

        public async Task AddPost(PostCreateDto postCreateModel)
        {
            if (postCreateModel.Title == null || postCreateModel.Content == null || postCreateModel.SubcategoryId == 0 || postCreateModel.UserId == 0)
                throw new BadRequestException("Invalid form data");
            await _postRepository.AddPost(new Post(postCreateModel.Title, postCreateModel.Content, postCreateModel.UserId, postCreateModel.SubcategoryId));
        }

        public async Task EditPost(int postId, PostEditDto postEditModel)
        {
            Post? post = await _postRepository.GetPostById(postId);

            if (post == null)
            {
                throw new BadRequestException("No post with such id exists.");
            }

            post.Title = postEditModel.Title!;
            post.Content = postEditModel.Content!;

            await _postRepository.EditPost(post);
        }

        public async Task DeletePost(int postId)
        {
            await _postRepository.DeletePost(postId);
        }

        public async Task ModerationRemovePost(int postId)
        {
            await _postRepository.RemovePost(postId);
        }

        public async Task UpdatePinStatus(int postId)
        {
            await _postRepository.UpdatePinStatus(postId);
        }

        public async Task UpdateLockStatus(int postId)
        {
            await _postRepository.UpdateLockStatus(postId);
        }
    }
}