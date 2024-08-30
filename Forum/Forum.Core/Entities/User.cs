using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Forum.Core.Enums;

namespace Forum.Core.Entities
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(16)]
		public string Username { get; set; }
		[Required]
		[EmailAddress]
		[MaxLength(100)]
		public string Email { get; set; }
		[Required]
		[MaxLength(100)]
		public string Password { get; set; }
		[Required]
		[MaxLength(20)]
		public string SelectedTitle { get; set; }
		public bool IsActive { get; set; }
		public bool IsBanned { get; set; } = false;
		[MaxLength(250)]
		public string Bio { get; set; }
		[MaxLength(20)]
		public string Location { get; set; }
		public DateTime LastLoggedIn { get; set; } = DateTime.Now;
		[Required]
		public Role Role { get; set; } = Role.Member;
		public DateTime CreatedAt { get; } = DateTime.Now;
		public List<Title> Titles { get; set; } = [];
		public List<Post> Posts { get; set; } = [];
		public List<Comment> Comments { get; set; } = [];
	}
}
