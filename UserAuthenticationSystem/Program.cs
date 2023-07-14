using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using UserAuthenticationSystem.Models;
using UserAuthenticationSystem.Repositories;
using UserAuthenticationSystem.Services;
using UserAuthenticationSystem.Types;

namespace UserAuthenticationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<UserAuthenticationSystemDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerDb")));

            builder.Services.AddScoped<IUserRepo, UserRepo>();

            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(CustomAuthenticationTypes.BasicAuthentication,null);
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
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers()
                .RequireAuthorization();

            app.Run();
        }
    }
}