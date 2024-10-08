using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class CommentGetDto
	{
        [Required]
        [MaxLength(500)]
        public string? Content { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
