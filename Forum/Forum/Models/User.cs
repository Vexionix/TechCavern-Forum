﻿namespace Forum.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Bio { get; set; }
		public string Title { get; set; }
		public string Role { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
