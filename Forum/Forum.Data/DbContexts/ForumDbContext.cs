using Forum.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data.DbContexts
{
	public class ForumDbContext : DbContext
	{
		public DbSet<Category> Categories {  get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Subcategory> Subcategories { get; set; }
		public DbSet<Title> Titles { get; set; }
		public DbSet<User> Users { get; set; }

		public ForumDbContext(DbContextOptions options): base(options) { }
	}
}
