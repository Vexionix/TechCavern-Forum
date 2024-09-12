using Forum.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data.DbContexts
{
	public class ForumDbContext : DbContext
	{
		public DbSet<Category> Categories { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Subcategory> Subcategories { get; set; }
		public DbSet<Title> Titles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }

		public ForumDbContext(DbContextOptions options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Title>()
				.HasData(new Title("Member") { Id = 1 });

			modelBuilder.Entity<RefreshToken>()
				.HasKey(refreshToken => new { refreshToken.UserId, refreshToken.Token });

			base.OnModelCreating(modelBuilder);
		}
	}
}
