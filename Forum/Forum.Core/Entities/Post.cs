using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Forum.Core.Entities
{
	public class Post
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Title { get; set; }
		[Required]
		[MaxLength(500)]
		public string Content { get; set; }
		public int Views { get; set; }
		public int Likes { get; set; }
		public DateTime CreatedAt { get; } = DateTime.Now;
		public bool IsEdited { get; set; } = false;
		public DateTime LastEditedAt { get; set; } = DateTime.Now;
		public bool IsDeleted { get; set; } = false;
		public bool IsRemoved { get; set; } = false;
		public User User {  get; set; }
		public int UserId { get; set; }
		public int SubcategoryId { get; set; }

		[ForeignKey("SubcategoryId")]
		public Subcategory Subcategory { get; set; }	
		public List<Comment> Comments { get; set; } = [];
	}
}
