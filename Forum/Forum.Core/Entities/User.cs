using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Forum.Core.Enums;
using System.Text.Json.Serialization;

namespace Forum.Core.Entities
{
	public class User
	{
		public User(string username, string email, string password, string selectedTitle, string bio, string location)
		{
			Username = username;
			Email = email;
			Password = password;
			SelectedTitle = selectedTitle;
			Bio = bio;
			Location = location;
			CreatedAt = DateTime.Now;
			Role = Role.Member;
		}

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
		public Role Role { get; set; }
		public DateTime CreatedAt { get; set; }
		public List<Title> Titles { get; set; } = [];
		public List<Post> Posts { get; set; } = [];
		public List<Comment> Comments { get; set; } = [];
	}
}
