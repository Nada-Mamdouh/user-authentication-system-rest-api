using Microsoft.EntityFrameworkCore;
using UserAuthenticationSystem.Models;
using UserAuthenticationSystem.Repositories;

namespace UserAuthenticationSystem
{
    public class Program
    { 
        string myConnectionStr;
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<UserAuthenticationSystemDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerDb")));
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}