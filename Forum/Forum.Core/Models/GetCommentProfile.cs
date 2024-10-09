using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class GetCommentProfile
	{
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Title { get; set; }
        public string? Content { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int PostId { get; set; }
    }
}
