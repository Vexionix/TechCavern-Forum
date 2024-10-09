using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class GetUserProfileData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? SelectedTitle { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        public string? Bio { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsBanned { get; set; }
        [Required]
        public int TotalPosts { get; set; }
        [Required]
        public int TotalComments { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime LastSeenOn { get; set; }
    }
}
