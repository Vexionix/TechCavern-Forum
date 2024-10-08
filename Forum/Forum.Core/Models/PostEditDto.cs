using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class PostEditDto
    {
        [Required]
        [MaxLength(50)]
        public string? Title { get; set; }
        [Required]
		[MaxLength(500)]
		public string? Content { get; set; }
	}
}
