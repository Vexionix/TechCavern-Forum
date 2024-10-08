using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Services;
using Forum.Models;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Entities;

namespace Forum.Application.Services
{
    public class UtilsService : IUtilsService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public UtilsService(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<StatisticsDto> GetStatistics()
        {
            int totalPosts = await _postRepository.GetTotalPostsNumber();
            int totalComments = await _commentRepository.GetTotalCommentsNumber();
            int totalUsers = await _userRepository.GetTotalUsersNumber();
            User? latestUser = await _userRepository.GetLatestUser();
            int latestUserId = latestUser == null ? 0 : latestUser.Id;
            string latestUsername = latestUser == null ? "" : latestUser.Username;

            return new StatisticsDto() { TotalPosts = totalPosts, TotalComments = totalComments, TotalUsers = totalUsers, LatestUserId = latestUserId, LatestUsername = latestUsername};   
        }
    }
}
