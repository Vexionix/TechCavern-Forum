using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class CommentCreateDto
	{
		[Required]
		[MaxLength(500)]
		public string? Content { get; set; }
		[Required]
		public int UserId { get; set; }
		[Required]
		public int PostId { get; set; }
	}
}
