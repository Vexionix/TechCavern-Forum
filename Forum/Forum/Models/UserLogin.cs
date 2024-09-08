using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class UserLogin
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
