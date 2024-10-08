using System.ComponentModel.DataAnnotations;

namespace Forum.Core.Models
{
    public class PostListElementDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public int Views { get; set; }
        [Required]
        public int CommentsCount { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int LatestCommentUserId { get; set; }
        [Required]
        public int SubcategoryId { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? LatestCommenterUsername { get; set; }
        [Required]
        public bool IsPinned { get; set; }
        [Required]
        public bool IsLocked { get; set; }
        [Required]
        public DateTime? CreatedAt { get; set; }
        [Required]
        public DateTime? LatestCommentPostedAt { get; set; }
    }
}
