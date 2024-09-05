using Forum.Core.Enums;

namespace Forum.Models
{
	public class UserDto
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Bio { get; set; }
		public string SelectedTitle { get; set; }
		public Role Role { get; set; }
		public DateTime CreatedAt { get; set; }
		public List<TitleDto> Titles { get; set; }
	}
}
