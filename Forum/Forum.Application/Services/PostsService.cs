using Forum.Core.Entities;
using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Models;

namespace Forum.API.Services
{
	public class PostsService : IPostsService
	{
		private readonly IPostRepository _postRepository;
		public PostsService(IPostRepository postRepository) 
		{
			_postRepository = postRepository;
		}

		public async Task<Post> GetPostById(int postId)
		{
			Post? post = await _postRepository.GetPostById(postId);

			if (post == null)
			{
				throw new BadRequestException("No post with such id exists.");
			}

			return post;
		}

		public async Task<int> GetPostsAddedToday()
		{
			int postsAddedToday = await _postRepository.GetPostsAddedToday();
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

			post.Content = postEditModel.Content;

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