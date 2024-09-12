using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class UserLogin
	{
		[Required]
		public required string Username { get; set; }
		[Required]
		public required string Password { get; set; }
	}
}
