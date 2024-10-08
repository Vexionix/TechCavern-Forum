using System.ComponentModel.DataAnnotations;

namespace Forum.Core.Models
{
    public class GetPostDisplayDataDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime? LatestActivityPostedAt { get; set; }
    }
}