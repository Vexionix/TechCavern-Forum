using Forum.Core.Interfaces.Repositories;
using Forum.Data.DbContexts;
using Forum.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Forum
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<ForumDbContext>(options => options.UseSqlServer(builder.Configuration["ForumConnectionString"]));

			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<IPostRepository, PostRepository>();
			builder.Services.AddScoped<ICommentRepository, CommentRepository>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
