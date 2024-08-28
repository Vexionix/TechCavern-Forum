using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Core.Entities
{
	public class Comment
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(250)]
		public string Content { get; set; }
		public int Likes { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public bool IsEdited { get; set; } = false;
		public DateTime LastEditedAt { get; set; } = DateTime.Now;
		public bool IsDeleted { get; set; } = false;
		public bool IsRemoved { get; set; } = false;
		public int UserId { get; set; }
		public int PostId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }
		[ForeignKey("PostId")]
		public Post Post { get; set; }
	}
}
