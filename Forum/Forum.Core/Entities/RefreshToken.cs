using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Core.Entities
{
	public class RefreshToken
	{
		public RefreshToken(int userId, string token, DateTime expiresAt)
		{
			UserId = userId;
			Token = token;
			ExpiresAt = expiresAt;
		}

		public int UserId { get; set; }
		public string Token { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime ExpiresAt { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }
	}
}
