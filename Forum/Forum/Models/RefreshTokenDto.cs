namespace Forum.API.Models
{
	public class RefreshTokenDto
	{
		public required string Token { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime ExpiresAt { get; set; }
    }
}
