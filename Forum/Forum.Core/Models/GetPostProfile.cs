using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class GetPostProfile
	{
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Title { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
