using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class GetCommentForPost
    {
        [Required]
        public int Id { get; set; }
		[Required]
		[MaxLength(500)]
		public string? Content { get; set; }
		[Required]
		public int UserId { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
		public DateTime CreatedAt { get; set; }
		[Required]
		public bool IsEdited {  get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public bool IsRemoved { get; set; }
        [Required]
        public DateTime? LastEditedAt { get; set; }
        [Required]
        public GetUserDataFoCommentAndPost? User { get; set; }
    }
}
