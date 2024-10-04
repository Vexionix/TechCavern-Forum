using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Core.Entities
{
	public class Comment
	{
		public Comment(string content, int userId, int postId)
		{
			Content = content;
			UserId = userId;
			PostId = postId;
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(250)]
		public string Content { get; set; }
		public int NumberOfLikes { get; set; } = 0;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public bool IsEdited { get; set; } = false;
		public DateTime LastEditedAt { get; set; } = DateTime.UtcNow;
		public bool IsDeleted { get; set; } = false;
		public bool IsRemovedByAdmin { get; set; } = false;
		public int UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }
		public int PostId { get; set; }
		[ForeignKey("PostId")]
		public Post Post { get; set; }
	}
}
