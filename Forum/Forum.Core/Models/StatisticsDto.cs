using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class StatisticsDto
    {
        [Required]
        public int TotalPosts { get; set; }
        [Required]
        public int TotalComments { get; set; }
        [Required]
        public int TotalUsers { get; set; }
        [Required]
        public int LatestUserId { get; set; }
        [Required]
        [MaxLength(25)]
        public string LatestUsername { get; set; }
	}
}
