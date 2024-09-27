using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Forum.Core.Entities
{
	public class Post
	{
		public Post(string title, string content, int userId, int subcategoryId)
		{
			Title = title;
			Content = content;
			UserId = userId;
			SubcategoryId = subcategoryId;
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Title { get; set; }
		[Required]
		[MaxLength(500)]
		public string Content { get; set; }
		public int NumberOfViews { get; set; } = 0;
		public int NumberOfLikes { get; set; } = 0;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public bool IsEdited { get; set; } = false;
		public DateTime LastEditedAt { get; set; } = DateTime.Now;
		public bool IsPinned { get; set; } = false;
		public bool IsLocked { get; set; } = false;
		public bool IsDeleted { get; set; } = false;
		public bool IsRemovedByAdmin { get; set; } = false;
		public int UserId { get; set; }
		[ForeignKey("UserId")]
		public User User {  get; set; }
		public int SubcategoryId { get; set; }

		[ForeignKey("SubcategoryId")]
		public Subcategory Subcategory { get; set; }
		public List<Comment> Comments { get; set; } = [];
	}
}
