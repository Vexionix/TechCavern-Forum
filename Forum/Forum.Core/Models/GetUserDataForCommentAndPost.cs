using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class GetUserDataFoCommentAndPost
	{
        [Required]
        public string? SelectedTitle { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsBanned { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime LastSeenOn { get; set; }
    }
}
