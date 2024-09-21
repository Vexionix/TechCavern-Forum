using Forum.API.Services;
using Forum.Application.Services;
using Forum.Core.Interfaces.Repositories;
using Forum.Core.Interfaces.Services;
using Forum.Data.DbContexts;
using Forum.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

			builder.Services.AddAuthentication().AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateAudience = false,
					ValidateIssuer = false,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!))
				};
			});

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowFrontend",
					policy =>
					{
						policy.WithOrigins("https://localhost:5173")
							  .AllowAnyHeader()
							  .AllowAnyMethod()
							  .AllowCredentials();
					});
			});

			builder.Services.AddDbContext<ForumDbContext>(options => options.UseSqlServer(builder.Configuration["ForumConnectionString"]));

			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<IPostRepository, PostRepository>();
			builder.Services.AddScoped<ICommentRepository, CommentRepository>();

			builder.Services.AddScoped<IPasswordService, PasswordService>();
			builder.Services.AddScoped<IValidationService, ValidationService>();
			builder.Services.AddScoped<ITokenService, TokenService>();

			builder.Services.AddScoped<IAuthService, AuthService>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseCors("AllowFrontend");

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
