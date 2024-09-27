using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class CommentEditDto
	{
		[Required]
		[MaxLength(500)]
		public string? Content { get; set; }
	}
}
