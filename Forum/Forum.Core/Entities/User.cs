using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Forum.Core.Entities
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string Username { get; set; }
		[Required]
		[EmailAddress]
		[MaxLength(100)]
		public string Email { get; set; }
		[Required]
		[MaxLength(25)]
		public string Password { get; set; }
		[Required]
		public string SelectedTitle { get; set; }
		public bool IsActive { get; set; }
		public bool IsBanned { get; set; } = false;
		[MaxLength(250)]
		public string Bio { get; set; }
		[MaxLength(20)]
		public string Location { get; set; }
		public DateTime LastLogin { get; set; } = DateTime.Now;
		[Required]
		public string Role { get; set; } = "Member";
		public DateTime CreatedAt { get; } = DateTime.Now;
		public List<Title> Titles { get; set; } = [];
		public List<Post> Posts { get; set; } = [];
		public List<Comment> Comments { get; set; } = [];
	}
}
