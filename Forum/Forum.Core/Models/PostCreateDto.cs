using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class PostCreateDto
	{
		[Required]
		[MinLength(5)]
		[MaxLength(50)]
		public string? Title {  get; set; }
		[Required]
		[MaxLength(500)]
		public string? Content { get; set; }
		[Required]
		public int UserId { get; set; }
		[Required]
		public int SubcategoryId { get; set; }
	}
}
